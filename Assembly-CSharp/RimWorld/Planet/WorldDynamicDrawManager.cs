using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002313 RID: 8979
	public class WorldDynamicDrawManager
	{
		// Token: 0x0600DDC7 RID: 56775 RVA: 0x00528E50 File Offset: 0x00527050
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + ((o != null) ? o.ToString() : null) + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.");
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x0600DDC8 RID: 56776 RVA: 0x00528EA0 File Offset: 0x005270A0
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + ((o != null) ? o.ToString() : null) + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.");
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x0600DDC9 RID: 56777 RVA: 0x00528EF0 File Offset: 0x005270F0
		public void DrawDynamicWorldObjects()
		{
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				return;
			}
			if (!DebugViewSettings.drawWorldObjects)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				foreach (WorldObject worldObject in this.drawObjects)
				{
					try
					{
						if (!worldObject.def.expandingIcon || ExpandableWorldObjectsUtility.TransitionPct(worldObject) < 1f)
						{
							if (!WorldRendererUtility.WorldBackgroundNow || worldObject.VisibleInBackground)
							{
								worldObject.Draw();
							}
						}
					}
					catch (Exception ex)
					{
						string str = "Exception drawing ";
						WorldObject worldObject2 = worldObject;
						string str2 = (worldObject2 != null) ? worldObject2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			catch (Exception ex3)
			{
				string str4 = "Exception drawing dynamic world objects: ";
				Exception ex4 = ex3;
				Log.Error(str4 + ((ex4 != null) ? ex4.ToString() : null));
			}
			this.drawingNow = false;
		}

		// Token: 0x04008E6B RID: 36459
		private readonly HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x04008E6C RID: 36460
		private bool drawingNow;
	}
}
