using System.Collections.ObjectModel;
using Frontier.Wif.Core.ComponentModel;
using Wif.Demo.Common;

namespace Wif.Demo
{
    /// <summary>
    /// Defines the <see cref="MainViewModel" />
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// Defines the _mobilePhoneCollection
        /// </summary>
        private ObservableCollection<MobilePhoneSingletonModel> _mobilePhoneCollection = new ObservableCollection<MobilePhoneSingletonModel>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the MobilePhoneCollection
        /// </summary>
        public ObservableCollection<MobilePhoneSingletonModel> MobilePhoneCollection
        {
            get => _mobilePhoneCollection;
            set => _mobilePhoneCollection = value;
        }

        #endregion

        public MainViewModel()
        {
            var model = MobilePhoneSingletonModel.Instance;
            model.Brand = Brand.Apple;
            model.NumberOfCPUCore = NumberOfCPUCore.Quad;
            model.RAM = RAM._4GB;
            model.ROM = ROM._64GB;
            model.ScreenResolution = ScreenResolution.FHD;
            model.ScreenSize = 5.2f;
            MobilePhoneCollection.Add(model);

            model = MobilePhoneSingletonModel.Instance;
            model.Brand = Brand.Apple;
            model.NumberOfCPUCore = NumberOfCPUCore.Quad;
            model.RAM = RAM._4GB;
            model.ROM = ROM._64GB;
            model.ScreenResolution = ScreenResolution.FHD;
            model.ScreenSize = 5.2f;
            MobilePhoneCollection.Add(model);

            model = MobilePhoneSingletonModel.Instance;
            model.Brand = Brand.Apple;
            model.NumberOfCPUCore = NumberOfCPUCore.Quad;
            model.RAM = RAM._4GB;
            model.ROM = ROM._64GB;
            model.ScreenResolution = ScreenResolution.FHD;
            model.ScreenSize = 5.2f;
            MobilePhoneCollection.Add(model);
        }
    }
}
