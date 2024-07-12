using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerInstance
{
    // 상호 컴포넌트간의 의존성이 존재하지 않는 모듈 형식의 컴포넌트 리스트
    [SerializeField] List<PlayerComponent> playerModuleList = new List<PlayerComponent>();
    // 모듈들을 직접적으로 사용하는 컴포넌트 리스트
    [SerializeField] List<PlayerComponent> playerComponentList = new List<PlayerComponent>();
    private Dictionary<Type, PlayerComponent> playerComponents = null;

    // 모듈 초기화 후 컴포넌트 초기화함
	private void InitPlayerComponents()
    {
        playerComponents = new Dictionary<Type, PlayerComponent>();
        playerModuleList.ForEach(RegistComponent);
        playerComponentList.ForEach(RegistComponent);
    }

    private void ReleasePlayerComponents()
    {
        playerComponentList.ForEach(i => i.Release());
        playerModuleList.ForEach(i => i.Release());
    }

    private void RegistComponent(PlayerComponent component)
    {
        Type type = component.GetType();
        if (playerComponents.ContainsKey(type))
            return;

        component.Init(this);
        playerComponents.Add(type, component);
    }

    public T GetPlayerComponent<T>() where T : PlayerComponent => playerComponents[typeof(T)] as T;
}
