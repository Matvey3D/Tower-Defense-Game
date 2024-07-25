﻿using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Services;
using Services.Input;
using Services.Mobs;

namespace Infrastructure.States
{
  public class BootstrapState : IState
  {
    private const string Initial = "Initial";
    private readonly GameStateMachine _stateMachine;
    private readonly InputService _inputService;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;
    
      
    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = services;
      _inputService = new StandaloneInputService();
      
      RegisterServices();
    }

    public void Enter() =>
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

    public void Exit()
    {
    }

    /// <summary>
    /// Артур не бей, надо будет поговорить
    /// </summary>
    private void RegisterServices()
    {
      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      _services.RegisterSingle<IInputService>(_inputService);
      _services.RegisterSingle<IAssets>(new AssetsProvider());
      _services.RegisterSingle<IGameFactory>(new GameFactory(AllServices.Container.Single<IAssets>()));
    }

    private void EnterLoadLevel() =>
      _stateMachine.Enter<LoadLevelState, string>("Main");
  }
}