using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A4D RID: 6733
	public static class MinifyUtility
	{
		// Token: 0x0600A8B7 RID: 43191 RVA: 0x003BE8C8 File Offset: 0x003BCAC8
		public static MinifiedThing MakeMinified(this Thing thing, DestroyMode destroyMode = DestroyMode.Vanish)
		{
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + ((thing != null) ? thing.ToString() : null) + " which is not minifiable.");
				return null;
			}
			thing.DeSpawnOrDeselect(destroyMode);
			if (thing.holdingOwner != null)
			{
				string str = "Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=";
				IThingHolder parentHolder = thing.ParentHolder;
				Log.Warning(str + ((parentHolder != null) ? parentHolder.ToString() : null));
				return null;
			}
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(thing);
			MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(thing.def.minifiedDef, null);
			minifiedThing.InnerThing = thing;
			if (blueprint_Install != null)
			{
				blueprint_Install.SetThingToInstallFromMinified(minifiedThing);
			}
			if (minifiedThing.InnerThing.stackCount > 1)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to minify ",
					thing.LabelCap,
					" with stack count ",
					minifiedThing.InnerThing.stackCount.ToString(),
					". Clamped stack count to 1."
				}));
				minifiedThing.InnerThing.stackCount = 1;
			}
			return minifiedThing;
		}

		// Token: 0x0600A8B8 RID: 43192 RVA: 0x003BE9C6 File Offset: 0x003BCBC6
		public static Thing TryMakeMinified(this Thing thing)
		{
			if (thing.def.Minifiable)
			{
				return thing.MakeMinified(DestroyMode.Vanish);
			}
			return thing;
		}

		// Token: 0x0600A8B9 RID: 43193 RVA: 0x003BE9E0 File Offset: 0x003BCBE0
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			if (minifiedThing != null)
			{
				return minifiedThing.InnerThing;
			}
			return outerThing;
		}

		// Token: 0x0600A8BA RID: 43194 RVA: 0x003BEA00 File Offset: 0x003BCC00
		public static MinifiedThing Uninstall(this Thing th)
		{
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + ((th != null) ? th.ToString() : null));
				return null;
			}
			bool flag = Find.Selector.IsSelected(th);
			Map map = th.Map;
			MinifiedThing minifiedThing = th.MakeMinified(DestroyMode.Vanish);
			GenPlace.TryPlaceThing(minifiedThing, th.Position, map, ThingPlaceMode.Near, null, null, null, 1);
			SoundDefOf.ThingUninstalled.PlayOneShot(new TargetInfo(th.Position, map, false));
			if (flag)
			{
				Find.Selector.Select(minifiedThing, false, false);
			}
			return minifiedThing;
		}
	}
}
