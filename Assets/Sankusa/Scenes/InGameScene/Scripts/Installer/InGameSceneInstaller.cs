using UnityEngine;
using Zenject;
using Sankusa.unity1week202205_2.InGameScene.View;
using GreyEngine.Basic;

namespace Sankusa.unity1week202205_2.InGameScene.Installer {
    public class InGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private CitizenViewManager citizenViewManager;
        [SerializeField] private GameStatusView gameStatusView;
        [SerializeField] private CommandBookReader commandBookReader;
        [SerializeField] private ResultView resultView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ResultView>()
                     .FromInstance(resultView)
                     .AsCached();

            Container.BindInterfacesAndSelfTo<CommandBookReader>()
                     .FromInstance(commandBookReader)
                     .AsCached();

            Container.BindInterfacesAndSelfTo<CitizenViewManager>()
                     .FromInstance(citizenViewManager)
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<GameStatusView>()
                     .FromInstance(gameStatusView)
                     .AsSingle();

            Container.BindInterfacesAndSelfTo<MainGameLoop>()
                     .AsSingle()
                     .NonLazy();

            Container.BindInterfacesAndSelfTo<RootGameLoop>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}