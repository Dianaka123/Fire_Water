using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
{
    public LevelsConfiguration LevelsConfiguration;
    public GameResources GameResources;

    public override void InstallBindings()
    {
        Container.BindInstance(LevelsConfiguration);
        Container.BindInstance(GameResources);
    }
}