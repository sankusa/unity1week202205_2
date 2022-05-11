using UnityEngine;
using Zenject;

namespace Sankusa.unity1week202205_2.Installer {
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoader>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}