using Assets.Scripts.Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
{
    public LevelsConfiguration LevelsConfiguration;

    public override void InstallBindings()
    {
        Container.BindInstance(LevelsConfiguration);
    }
}