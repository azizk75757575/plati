using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/UI/UI Main Menu")]
	public class UIMainMenu : Singleton<UIMainMenu>
	{
		public enum Screen
		{
			Title,
			FileSelect,
			LevelSelect,
		}

		[Tooltip("Reference to the title screen UI container.")]
		public UIContainer titleScreen;

		[Tooltip("Reference to the file select screen UI container.")]
		public UIContainer fileSelectScreen;

		[Tooltip("Reference to the level select screen UI container.")]
		public UIContainer levelSelectScreen;

		protected Screen m_currentScreen = Screen.Title;
		protected Dictionary<Screen, UIContainer> m_screens;

		protected virtual void Start()
		{
			InitializeCallbacks();
			InitializeScreens();
		}

		protected virtual void InitializeCallbacks()
		{
			Game.instance.onLoadState.AddListener(OnLoadState);
		}

		protected virtual void InitializeScreens()
		{
			m_screens = new Dictionary<Screen, UIContainer>
			{
				{ Screen.Title, titleScreen },
				{ Screen.FileSelect, fileSelectScreen },
				{ Screen.LevelSelect, levelSelectScreen },
			};

			foreach (var screen in m_screens.Values)
				screen.SetActive(false);

			var initialScreen = Game.instance.dataLoaded ? Screen.LevelSelect : Screen.Title;
			m_currentScreen = initialScreen;
			m_screens[m_currentScreen].SetActive(true);
			m_screens[m_currentScreen].Show();
		}

		protected virtual void OnLoadState(int _) => ChangeTo(Screen.LevelSelect);

		public void ChangeToPrevious()
		{
			var index = (int)m_currentScreen - 1;
			var totalScreens = System.Enum.GetValues(typeof(Screen)).Length;
			index = Mathf.Clamp(index, 0, totalScreens - 1);
			ChangeTo((Screen)index);
		}

		public void ChangeTo(Screen screen)
		{
			if (m_currentScreen == screen)
				return;

			m_screens[m_currentScreen]
				.Hide(() =>
				{
					m_screens[m_currentScreen].SetActive(false);
					m_currentScreen = screen;
					m_screens[m_currentScreen].SetActive(true);
					m_screens[m_currentScreen].Show();
				});
		}
	}
}
