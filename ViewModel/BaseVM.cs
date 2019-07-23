using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.ViewModel
{
    class BaseVM
    {
        public enum ViewModels { Login, Registration, PlayListEditor, PlayList, LocationMap, AreaChooser};
        private Dictionary<ViewModels, IVM> ViewModelToType = new Dictionary<ViewModels, IVM>();
        private static BaseVM Instance = null;

        public static BaseVM GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new BaseVM();
                }
                return Instance;
            }
        }

        public IVM _LoginVM { get; } = new LoginVM();
        public IVM _RegistrationVM { get; } = new RegistrationVM();

        public IVM _LocationMapChoosenVM { get; } = new LocationMapChooserVM();


        public IVM _PlayListEditorVM { get; } = new PlayListEditorVM();



        public IVM _PlayListVM { get; } = new PlayListVM();
        public IVM _CountryChooserVM { get; } = new CountryChooserVM();

        /// <summary>
        /// baseVm consturctor.private for singelton.
        /// </summary>
        private BaseVM()
        {
            createDict();
        }


        /// <summary>
        /// put the viewmodels into the dictionary with enums.
        /// </summary>
        public void createDict()
        {
            ViewModelToType.Add(ViewModels.Login, _LoginVM);
            ViewModelToType.Add(ViewModels.Registration, _RegistrationVM);
            ViewModelToType.Add(ViewModels.PlayListEditor, _PlayListEditorVM);
            ViewModelToType.Add(ViewModels.PlayList, _PlayListVM);
            ViewModelToType.Add(ViewModels.LocationMap, _LocationMapChoosenVM);
            ViewModelToType.Add(ViewModels.AreaChooser, _CountryChooserVM);
        }

        /// <summary>
        /// move paramaters from viewmodels,by enums.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public void SendParam(ViewModels src, ViewModels dst)
        {
            IVM srcVM, dstVM;
            ViewModelToType.TryGetValue(src, out srcVM);
            ViewModelToType.TryGetValue(dst, out dstVM);
            dstVM.RecivedParameters(srcVM.GetParameters());
        }
    }
}
