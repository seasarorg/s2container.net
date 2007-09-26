/*
 * Created by: 
 * Created: 2007”N7ŒŽ2“ú
 */

using Seasar.Dxo.Annotation;

namespace Seasar.Tests.Dxo 
{
    public interface IBeanToBeanDxo
    {
        [DatePattern("yyyyMMdd")]
        TargetBean ConvertBeanToTarget(BeanA source);
    }
}