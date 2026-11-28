using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001EB RID: 491
	public static class GameComponentUtility
	{
		// Token: 0x06000DEB RID: 3563 RVA: 0x00048120 File Offset: 0x00046320
		public static void GameComponentUpdate()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00048174 File Offset: 0x00046374
		public static void GameComponentTick()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x000481C8 File Offset: 0x000463C8
		public static void GameComponentOnGUI()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0004821C File Offset: 0x0004641C
		public static void FinalizeInit()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00048270 File Offset: 0x00046470
		public static void StartedNewGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].StartedNewGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000482C4 File Offset: 0x000464C4
		public static void LoadedGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].LoadedGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}
	}
}
