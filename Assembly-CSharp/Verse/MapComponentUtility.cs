using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200029E RID: 670
	public static class MapComponentUtility
	{
		// Token: 0x06001489 RID: 5257 RVA: 0x00075758 File Offset: 0x00073958
		public static void MapComponentUpdate(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x000757A8 File Offset: 0x000739A8
		public static void MapComponentTick(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000757F8 File Offset: 0x000739F8
		public static void MapComponentOnGUI(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00075848 File Offset: 0x00073A48
		public static void MapComponentOnDraw(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentDraw();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00075898 File Offset: 0x00073A98
		public static void FinalizeInit(Map map)
		{
			List<MapComponent> components = map.components;
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

		// Token: 0x0600148E RID: 5262 RVA: 0x000758E8 File Offset: 0x00073AE8
		public static void MapGenerated(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapGenerated();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00075938 File Offset: 0x00073B38
		public static void MapRemoved(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapRemoved();
				}
				catch (Exception ex)
				{
					string str = "Could not notify map component: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}
	}
}
