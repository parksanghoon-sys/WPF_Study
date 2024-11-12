using Indigator.Lib.GeometryUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Indigator.Lib.DigitalFonts
{
    /// <summary>
    /// 여러 세그먼트로 구성된 디지털 문자 양식을 정의
    /// </summary>
    public abstract class DigitalFont : ParametricCache<IReadOnlyList<Part>>
    {
        private DigitalCharacterSet characterSet;
        private double size = 50;
        private double widthRatio = 1;
        private double slantAngle = 0;
        private Size actualSize;
        private double width;
        /// <summary>
        /// 각 문자에 대한 사용자 세그먼트 상태 이진코드 Dic
        /// </summary>
        public Dictionary<char, long> CustomBinaryCodes {  get; set; }                        
        private readonly static Cache<Guid, DigitalCharacterSet> cache = new Cache<Guid, DigitalCharacterSet>();
        /// <summary>
        /// 디지털 문자의 세로 크기를 가져오거나 설정
        /// </summary>
        [DefaultValue(50d)]
        public double Size { get => size; set => SetParameter(ref size, value); }
        /// <summary>
        /// 세로 키기 기준 가로 비율
        /// </summary>
        [DefaultValue(1d)]
        public double WidthRatio { get => widthRatio; set => SetParameter(ref widthRatio, value); }
        /// <summary>
        /// 디지털 문자의 기울임 각도를 가져오거나 설정합니다.
        /// </summary>
        [DefaultValue(0d)]
        public double SlantAngle { get => slantAngle; set => SetParameter(ref slantAngle, value); }

        /// <summary>
        /// 디지털 문자의 최종 적용 크기를 가져옵니다. 여기서 Size.Width 값은 기울임 각도가 적용될 때 발생한 확장 영역을 포함합니다.
        /// </summary>
        public Size ActualSize { get => actualSize; private set => SetProperty(ref actualSize, value); }
        /// <summary>
        /// 디지털 문자의 기울임 각도를 적용하지 않았을 때의 가로 크기를 가져옵니다. 
        /// </summary>
        public double Width { get => width; protected set => SetProperty(ref width, value); }
        /// <summary>
        /// 대상 문자에 대한 세그먼트 상태 이진코드를 가져온다, 세그먼트 상태의 순서는 LSB 부터
        /// </summary>
        /// <param name="character">문자</param>
        /// <returns>세그먼트 상태 이진 코드</returns>
        public long GetBinaryCode(char character)
            => CustomBinaryCodes?.TryGetValue(character, out var code) == true ? code : GetDefaultBinanryCode(character);
        public IEnumerable<Part> GetCharactoerSegments(char character, DigitalSegmentFilter segmentFilter)
        {
            if (characterSet is null)
                characterSet = cache.GetValue(Hash, () => new DigitalCharacterSet(Segments));

            switch (segmentFilter)
            {
                case DigitalSegmentFilter.InactiveOnly:
                    return characterSet.GetDrawings(GetBinaryCode(character)).Inactive;
                case DigitalSegmentFilter.All:
                    return characterSet.GetDrawings(GetBinaryCode(' ')).Inactive;
                default:
                    return characterSet.GetDrawings(GetBinaryCode(character)).Active;
            }
        }

        /// <summary>
        /// 세그먼트 파트 목록을 가져온다.
        /// </summary>
        public IReadOnlyList<Part> Segments => GetObject();
        protected abstract long GetDefaultBinanryCode(char character);
        /// <summary>
        /// 디지털 문자 양식의 크기를 측정한다
        /// </summary>
        /// <returns>크기</returns>
        protected abstract Size Measure();
        protected override void OnInvalidated(Guid hash)
        {
            characterSet = null;
            cache.Release(hash);
            ActualSize = Measure();
        }
        /// <summary>
        /// 파라미터가 같은지 체크
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool EqualsParameters(ParametricCache<IReadOnlyList<Part>> target)
        {
            return base.EqualsParameters(target)
                && target is DigitalFont parameter
                && size.Equals(parameter.Size)
                && widthRatio.Equals(parameter.widthRatio)
                && slantAngle.Equals(parameter.slantAngle);
        }
        protected override StringBuilder OnGenerateParametersString(StringBuilder stringBuilder)
                  => base.OnGenerateParametersString(stringBuilder)
            .Append(',').Append(slantAngle)
            .Append(',').Append(size)
            .Append(',').Append(widthRatio);

        ~DigitalFont() 
        {
            cache.Release(Hash); 
        }
        class DigitalCharacterDrawings
        {
            public IEnumerable<Part> Active;
            public IEnumerable<Part> Inactive;

            public DigitalCharacterDrawings(IEnumerable<Part> active, IEnumerable<Part> inactive)
            {
                Active = active;
                Inactive = inactive;
            }
        }
        class DigitalCharacterSet
        {
            private readonly Part[] segments;
            private readonly Dictionary<long, DigitalCharacterDrawings> cache = new Dictionary<long, DigitalCharacterDrawings>();
            public DigitalCharacterSet(IEnumerable<Part> segments)
            {
                this.segments = segments.ToArray();
            }
            public DigitalCharacterDrawings GetDrawings(long code)
            {
                if (cache.TryGetValue(code, out var character) == false)
                {
                    int index = 0;
                    var active = new List<Part>();
                    var inactive = new List<Part>();
                    foreach (var part in segments)
                    {
                        if ((code >> index & 1) == 1)
                            active.Add(part);
                        else
                            inactive.Add(part);
                        index++;
                    }
                    character = new DigitalCharacterDrawings(new ReadOnlyCollection<Part>(active), new ReadOnlyCollection<Part>(inactive));
                    cache[code] = character;
                }
                return character;
            }
        }

    }
}
