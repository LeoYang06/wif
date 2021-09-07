using System.ComponentModel;

namespace Wif.Demo
{
    /// <summary>
    /// 表示Demo视图类别枚举。
    /// </summary>
    public enum DemoViewCategories
    {
        [Description(nameof(BindingDemoView))]
        BindingDemoView,

        [Description(nameof(FileHelperDemoView))]
        FileHelperDemoView
    }
}