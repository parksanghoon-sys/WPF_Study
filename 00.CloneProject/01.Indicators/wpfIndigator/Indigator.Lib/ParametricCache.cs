using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Indigator.Lib
{
    public abstract class ParametricCache<T> : INotifyPropertyChanged where T : class
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler ParameterChanged;

        private readonly static Cache<Guid, T> _cache = new Cache<Guid, T>();
        private T value;
        private Guid hash;
        private bool isUpdating;
        private bool isUpdated;
        private bool isValid;
        /// <summary>
        /// 파라미터에 의해 정의된 해시 값
        /// </summary>
        public Guid Hash { get => hash; set => SetProperty(ref hash, value); }
        /// <summary>
        /// BeginUpdate 메서드를 호출하여 현재 파라미터를 수정하는 여부를 가져온다 , EndUpdate 메서드를 호출시 false
        /// </summary>
        public bool IsUpdating { get => isUpdating; set => SetProperty(ref isUpdating, value); }
        /// <summary>
        /// 생성자
        /// </summary>
        protected ParametricCache() => Invalidate();

        /// <summary>
        /// 종료자
        /// </summary>
        ~ParametricCache()
        {
            _cache.Release(hash);
        }
        /// <summary>
        /// 파라미터 업데이트를 시작합니다. 업데이트가 끝났을 때 EndUpdate 메서드를 호출해야 합니다.
        /// </summary>
        public void BeginUpdate()
        {
            IsUpdating = true;
            isUpdated = false;
        }
        /// <summary>
        /// 파라미터 업데이트를 종료합니다. 값이 바뀐 파라미터가 하나라도 있을 경우, 기존의 Hash를 무효화 합니다.
        /// </summary>
        public void EndUpdate()
        {
            if (isUpdated)
                RaiseParameterChanged();
            IsUpdating = false;
        }
        /// <summary>
        /// 대상 캐시의 모든 파라미터가 캐시의 파라미터와 같은 여부
        /// </summary>
        /// <param name="target">대상 캐시</param>
        /// <returns>모든 파라미터의 일치여부</returns>
        public virtual bool EqualsParameters(ParametricCache<T> target)
            => target is ParametricCache<T> parameters && parameters.GetType().Equals(GetType());
        protected T GetObject()
        {
            if(isValid == false)
            {
                value = _cache.GetValue(hash, () => OnCreate());
                isValid = true;
            }
            return value;
        }
        /// <summary>
        /// 파라미터들에 의해 정의된 해시가 파라미터 변경에 의해 무효화 되었을 때 호출됩니다.
        /// </summary>
        /// <param name="hash">무효화 된 파라미터들에 의해 정의된 해시</param>
        protected virtual void OnInvalidated(Guid hash) { }
        /// <summary>
        /// 속성에 값을 설정한다
        /// </summary>
        /// <typeparam name="TProperty">속성 형식</typeparam>
        /// <param name="target">속성 저장 변수</param>
        /// <param name="value">설정할 값</param>
        /// <param name="propertyName">속성이름</param>
        /// <returns>현재 설정으로 인해 속성이 바뀐 여부</returns>
        protected bool SetProperty<TProperty>(ref TProperty target, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if(EqualityComparer<TProperty>.Default.Equals(target, value) == false)
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
        protected bool SetParameter<TProperty>(ref TProperty target, TProperty value, [CallerMemberName] string propertyName = null)
        {
            if(SetProperty(ref target, value, propertyName))
            {
                if (isUpdating)
                    isUpdated = true;
                else
                    RaiseParameterChanged();
                return true;
            }
            return false;
        }
        /// <summary>
        /// ParameterChanged 이벤트를 발생시킵니다.
        /// </summary>
        protected void RaiseParameterChanged()
        {
            Invalidate();
            ParameterChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 파라미터를 이용하여 객체를 생성할 때 호출됩니다.
        /// </summary>
        /// <returns>캐시할 객체</returns>
        protected abstract T OnCreate();
        /// <summary>
        /// 파라미터를 나타내는 문자열을 생성할 때 호출됩니다. 해당 문자열은 Hash를 생성할 때 사용되므로, 반드시 모든 파라미터의 값을 콤마 등의 구분기호로 나누어 문자열에 포합해야 합니다.
        /// </summary>
        /// <param name="stringBuilder">StringBuilder 객체</param>
        /// <returns>파라미터 문자열</returns>
        protected virtual StringBuilder OnGenerateParametersString(StringBuilder stringBuilder) => stringBuilder.Append(GetType());
        private void Invalidate()
        {
            isValid = false;
            _cache.Release(hash);
            OnInvalidated(hash);
            Hash = new Guid(MD5.ComputeHash(Encoding.UTF8.GetBytes(OnGenerateParametersString(new StringBuilder()).ToString())));
        }    
       

    }
}
