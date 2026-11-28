using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200233F RID: 9023
	public static class WorldComponentUtility
	{
		// Token: 0x0600DF07 RID: 57095 RVA: 0x0052F470 File Offset: 0x0052D670
		public static void WorldComponentUpdate(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600DF08 RID: 57096 RVA: 0x0052F4C0 File Offset: 0x0052D6C0
		public static void WorldComponentTick(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600DF09 RID: 57097 RVA: 0x0052F510 File Offset: 0x0052D710
		public static void WorldComponentOnGUI(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x0600DF0A RID: 57098 RVA: 0x0052F560 File Offset: 0x0052D760
		public static void FinalizeInit(World world, bool fromLoad)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit(fromLoad);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}
	}
}
