using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020005A4 RID: 1444
	public static class ThingOwnerUtility
	{
		// Token: 0x06002B4A RID: 11082 RVA: 0x001073D0 File Offset: 0x001055D0
		public static bool ThisOrAnyCompIsThingHolder(this ThingDef thingDef)
		{
			if (typeof(IThingHolder).IsAssignableFrom(thingDef.thingClass))
			{
				return true;
			}
			for (int i = 0; i < thingDef.comps.Count; i++)
			{
				if (typeof(IThingHolder).IsAssignableFrom(thingDef.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x00107434 File Offset: 0x00105634
		public static ThingOwner TryGetInnerInteractableThingOwner(this Thing thing)
		{
			IThingHolder thingHolder = thing as IThingHolder;
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingHolder != null)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				if (directlyHeldThings != null)
				{
					return directlyHeldThings;
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					IThingHolder thingHolder2 = allComps[i] as IThingHolder;
					if (thingHolder2 != null)
					{
						ThingOwner directlyHeldThings2 = thingHolder2.GetDirectlyHeldThings();
						if (directlyHeldThings2 != null)
						{
							return directlyHeldThings2;
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			if (thingHolder != null)
			{
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
				{
					ThingOwner directlyHeldThings3 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
					if (directlyHeldThings3 != null)
					{
						ThingOwnerUtility.tmpHolders.Clear();
						return directlyHeldThings3;
					}
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps2 = thingWithComps.AllComps;
				for (int j = 0; j < allComps2.Count; j++)
				{
					IThingHolder thingHolder3 = allComps2[j] as IThingHolder;
					if (thingHolder3 != null)
					{
						thingHolder3.GetChildHolders(ThingOwnerUtility.tmpHolders);
						if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
						{
							ThingOwner directlyHeldThings4 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
							if (directlyHeldThings4 != null)
							{
								ThingOwnerUtility.tmpHolders.Clear();
								return directlyHeldThings4;
							}
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			return null;
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x00107562 File Offset: 0x00105762
		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			return ThingOwnerUtility.SpawnedParentOrMe(holder) != null;
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x00107570 File Offset: 0x00105770
		public static Thing SpawnedParentOrMe(IThingHolder holder)
		{
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Spawned)
				{
					return thing;
				}
				ThingComp thingComp = holder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x001075C0 File Offset: 0x001057C0
		public static IntVec3 GetRootPosition(IThingHolder holder)
		{
			IntVec3 result = IntVec3.Invalid;
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Position.IsValid)
				{
					result = thing.Position;
				}
				else
				{
					ThingComp thingComp = holder as ThingComp;
					if (thingComp != null && thingComp.parent.Position.IsValid)
					{
						result = thingComp.parent.Position;
					}
				}
				holder = holder.ParentHolder;
			}
			return result;
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x00107630 File Offset: 0x00105830
		public static Map GetRootMap(IThingHolder holder)
		{
			while (holder != null)
			{
				Map map = holder as Map;
				if (map != null)
				{
					return map;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x00107658 File Offset: 0x00105858
		public static PlanetTile GetRootTile(IThingHolder holder)
		{
			while (holder != null)
			{
				WorldObject worldObject = holder as WorldObject;
				if (worldObject != null && worldObject.Tile.Valid)
				{
					return worldObject.Tile;
				}
				holder = holder.ParentHolder;
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x00107698 File Offset: 0x00105898
		public static bool ContentsSuspended(IThingHolder holder)
		{
			while (holder != null)
			{
				if (!(holder is Building_CryptosleepCasket))
				{
					ISuspendableThingHolder suspendableThingHolder = holder as ISuspendableThingHolder;
					if (suspendableThingHolder == null || !suspendableThingHolder.IsContentsSuspended)
					{
						holder = holder.ParentHolder;
						continue;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x001076CF File Offset: 0x001058CF
		public static bool ContentsInCryptosleep(IThingHolder holder)
		{
			while (holder != null)
			{
				if (holder is Building_CryptosleepCasket)
				{
					return true;
				}
				holder = holder.ParentHolder;
			}
			return false;
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x001076E9 File Offset: 0x001058E9
		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x00107724 File Offset: 0x00105924
		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x0010773C File Offset: 0x0010593C
		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0010774A File Offset: 0x0010594A
		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x00107754 File Offset: 0x00105954
		public static void AppendThingHoldersFromThings(List<IThingHolder> outThingsHolders, IList<Thing> container)
		{
			if (container == null)
			{
				return;
			}
			int i = 0;
			int count = container.Count;
			while (i < count)
			{
				IThingHolder thingHolder = container[i] as IThingHolder;
				if (thingHolder != null)
				{
					outThingsHolders.Add(thingHolder);
				}
				ThingWithComps thingWithComps = container[i] as ThingWithComps;
				if (thingWithComps != null)
				{
					List<ThingComp> allComps = thingWithComps.AllComps;
					for (int j = 0; j < allComps.Count; j++)
					{
						IThingHolder thingHolder2 = allComps[j] as IThingHolder;
						if (thingHolder2 != null)
						{
							outThingsHolders.Add(thingHolder2);
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x001077D9 File Offset: 0x001059D9
		public static bool AnyParentIs<T>(Thing thing) where T : class, IThingHolder
		{
			return ThingOwnerUtility.GetAnyParent<T>(thing) != null;
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x001077EC File Offset: 0x001059EC
		public static T GetAnyParent<T>(Thing thing) where T : class, IThingHolder
		{
			T t = thing as T;
			if (t != null)
			{
				return t;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				T t2 = parentHolder as T;
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x00107844 File Offset: 0x00105A44
		public static Thing GetFirstParentThing(Thing thing)
		{
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				Thing thing2 = parentHolder as Thing;
				if (thing2 != null)
				{
					return thing2;
				}
				ThingComp thingComp = parentHolder as ThingComp;
				if (thingComp != null)
				{
					return thingComp.parent;
				}
			}
			return null;
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x00107884 File Offset: 0x00105A84
		public static Thing GetFirstSpawnedParentThing(Thing thing)
		{
			if (thing.Spawned)
			{
				return thing;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				Thing thing2 = parentHolder as Thing;
				if (thing2 != null && thing2.Spawned)
				{
					return thing2;
				}
				ThingComp thingComp = parentHolder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
			}
			return null;
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x001078E4 File Offset: 0x00105AE4
		public static void GetAllThingsRecursively(IThingHolder holder, List<Thing> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null)
		{
			outThings.Clear();
			if (passCheck != null && !passCheck(holder))
			{
				return;
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpStack.Push(holder);
			while (ThingOwnerUtility.tmpStack.Count != 0)
			{
				IThingHolder thingHolder = ThingOwnerUtility.tmpStack.Pop();
				if (allowUnreal || ThingOwnerUtility.AreImmediateContentsReal(thingHolder))
				{
					ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						outThings.AddRange(directlyHeldThings);
					}
				}
				ThingOwnerUtility.tmpHolders.Clear();
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				for (int i = 0; i < ThingOwnerUtility.tmpHolders.Count; i++)
				{
					if (passCheck == null || passCheck(ThingOwnerUtility.tmpHolders[i]))
					{
						ThingOwnerUtility.tmpStack.Push(ThingOwnerUtility.tmpHolders[i]);
					}
				}
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpHolders.Clear();
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x001079BC File Offset: 0x00105BBC
		public static void GetAllThingsRecursively<T>(Map map, ThingRequest request, List<T> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null, bool alsoGetSpawnedThings = true) where T : Thing
		{
			outThings.Clear();
			if (alsoGetSpawnedThings)
			{
				List<Thing> list = map.listerThings.ThingsMatching(request);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i] as T;
					if (t != null)
					{
						outThings.Add(t);
					}
				}
			}
			ThingOwnerUtility.tmpMapChildHolders.Clear();
			map.GetChildHolders(ThingOwnerUtility.tmpMapChildHolders);
			for (int j = 0; j < ThingOwnerUtility.tmpMapChildHolders.Count; j++)
			{
				ThingOwnerUtility.tmpThings.Clear();
				ThingOwnerUtility.GetAllThingsRecursively(ThingOwnerUtility.tmpMapChildHolders[j], ThingOwnerUtility.tmpThings, allowUnreal, passCheck);
				for (int k = 0; k < ThingOwnerUtility.tmpThings.Count; k++)
				{
					T t2 = ThingOwnerUtility.tmpThings[k] as T;
					if (t2 != null && request.Accepts(t2))
					{
						outThings.Add(t2);
					}
				}
			}
			ThingOwnerUtility.tmpThings.Clear();
			ThingOwnerUtility.tmpMapChildHolders.Clear();
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x00107AC8 File Offset: 0x00105CC8
		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal, null);
			return list;
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x00107AE5 File Offset: 0x00105CE5
		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00107B00 File Offset: 0x00105D00
		public static bool TryGetFixedTemperature(IThingHolder holder, Thing forThing, out float temperature)
		{
			if (holder is Pawn_InventoryTracker && forThing.TryGetComp<CompHatcher>() != null)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompLaunchable || holder is ActiveTransporterInfo || holder is TravellingTransporters)
			{
				temperature = 14f;
				return true;
			}
			if (holder is Settlement_TraderTracker || holder is TradeShip)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompTransporter)
			{
				temperature = 14f;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x04001D7E RID: 7550
		private static readonly Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		// Token: 0x04001D7F RID: 7551
		private static readonly List<IThingHolder> tmpHolders = new List<IThingHolder>();

		// Token: 0x04001D80 RID: 7552
		private static readonly List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04001D81 RID: 7553
		private static readonly List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();
	}
}
