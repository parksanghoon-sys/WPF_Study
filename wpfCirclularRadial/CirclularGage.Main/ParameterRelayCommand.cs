using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CirclularGage.Main
{
    public class ParameterRelayCommand : ICommand
    {
        /**
       * @brief  ParameterRelayCommand 생성자 함수.
       * @param Action<object> execute : 실행 이벤트 가 들어왔을때 동작하기 위한 Action함수를 등록
       * @return
       * @exception
       */
        public ParameterRelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /**
         * @brief ParameterRelayCommand 생성자 함수.
         * @param   Action<object> execute : 실행 이벤트 가 들어왔을때 동작하기 위한 Action함수를 등록, Predicate<object> canExecute : Control이 실행 가능 한지 불가능한지 판단하는 함수 등록
         * @return
         * @exception
         */
        public ParameterRelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /**
       * @brief 현재 컨트롤이 실행 가능한지 불가능한지 확인 하는 함수
       * @param   object parameter
       * @return  true : 실행가능 , false : 실행 불가능
       * @exception
       */
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /**
         * @brief 현재 컨트롤이 실행 가능한지 불가능한지 확인 하는 함수
         * @param   object parameter
         * @return  true : 실행가능 , false : 실행 불가능
         * @exception
         */
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        /**
         * @brief CanExecuteChaged 이벤트를 발생시킵니다
         * @param   
         * @return  
         * @exception
         */
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /**
        * @brief CanExecuteChaged 이벤트를 발생시킵니다
        * @param   
        * @return  
        * @exception
        */
        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }



        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;
    }
}
