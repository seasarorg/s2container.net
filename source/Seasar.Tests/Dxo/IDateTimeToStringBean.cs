using Seasar.Dxo.Annotation;

namespace Seasar.Tests.Dxo
{
    public interface IDateTimeToStringBean
    {
        StringBean ConvertBeanToTargetWithoutDatePattern(DateTimeBean source);

        [DatePattern("yyyyMMdd")]
        StringBean ConvertBeanToTarget1(DateTimeBean source);

        [DatePattern("yyyy-MM-dd")]
        StringBean ConvertBeanToTarget2(DateTimeBean source);
    }
}