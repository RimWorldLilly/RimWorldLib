using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200059D RID: 1437
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable where T : Thing
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x00105CDC File Offset: 0x00103EDC
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x170008B9 RID: 2233
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x00105CF2 File Offset: 0x00103EF2
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x170008BB RID: 2235
		T IList<!0>.this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x00105D19 File Offset: 0x00103F19
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00105D1C File Offset: 0x00103F1C
		public ThingOwner()
		{
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00105D2F File Offset: 0x00103F2F
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x00105D43 File Offset: 0x00103F43
		public ThingOwner(IThingHolder owner, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : base(owner)
		{
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x00105D57 File Offset: 0x00103F57
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : base(owner, oneStackOnly, contentsLookMode, removeContentsIfDestroyed)
		{
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x00105D70 File Offset: 0x00103F70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<T>(ref this.innerList, "innerList", true, this.contentsLookMode, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int num = this.innerList.RemoveAll(delegate(T x)
				{
					if (x != null)
					{
						MinifiedThing minifiedThing = x as MinifiedThing;
						return minifiedThing != null && minifiedThing.InnerThing == null;
					}
					return true;
				});
				if (num > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
					defaultInterpolatedStringHandler.AppendLiteral("ThingOwner removed ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(" invalid entries during PostLoadInit.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					if (this.innerList[i] != null)
					{
						this.innerList[i].holdingOwner = this;
					}
				}
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x00105E5C File Offset: 0x0010405C
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00105E69 File Offset: 0x00104069
		public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (!(item is T))
			{
				return 0;
			}
			return base.GetCountCanAccept(item, canMergeWithExistingStacks);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00105E80 File Offset: 0x00104080
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (count <= 0)
			{
				return 0;
			}
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return 0;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + ((item != null) ? item.ToString() : null) + " to ThingOwner but this item is already here.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					count.ToString(),
					" of ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return 0;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return 0;
			}
			int stackCount = item.stackCount;
			int num = Mathf.Min(stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.TryAdd((T)((object)thing), canMergeWithExistingStacks))
			{
				CompPushable compPushable = item.TryGetComp<CompPushable>();
				if (compPushable != null)
				{
					Pawn pawn = this.owner as Pawn;
					if (pawn != null)
					{
						compPushable.OnStartedCarrying(pawn);
					}
				}
				return num;
			}
			if (thing != item)
			{
				int result = stackCount - item.stackCount - thing.stackCount;
				item.TryAbsorbStack(thing, false);
				return result;
			}
			return stackCount - item.stackCount;
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00105FC8 File Offset: 0x001041C8
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return false;
			}
			T t = item as T;
			if (t == null)
			{
				return false;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return false;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return false;
			}
			if (canMergeWithExistingStacks)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					T t2 = this.innerList[i];
					if (t2.CanStackWith(item))
					{
						int num = Mathf.Min(item.stackCount, t2.def.stackLimit - t2.stackCount);
						if (num > 0)
						{
							Thing other = item.SplitOff(num);
							int stackCount = t2.stackCount;
							t2.TryAbsorbStack(other, true);
							if (t2.stackCount > stackCount)
							{
								base.NotifyAddedAndMergedWith(t2, t2.stackCount - stackCount);
							}
							if (item.Destroyed || item.stackCount == 0)
							{
								return true;
							}
						}
					}
				}
			}
			if (this.Count >= this.maxStacks)
			{
				return false;
			}
			item.holdingOwner = this;
			this.innerList.Add(t);
			this.NotifyAdded(t);
			return true;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0010618C File Offset: 0x0010438C
		protected override void NotifyAdded(Thing item)
		{
			IThingHolderEvents<T> thingHolderEvents = this.owner as IThingHolderEvents<T>;
			if (thingHolderEvents != null)
			{
				thingHolderEvents.Notify_ItemAdded(item as T);
			}
			base.NotifyAdded(item);
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x001061C0 File Offset: 0x001043C0
		protected override void NotifyRemoved(Thing item)
		{
			IThingHolderEvents<T> thingHolderEvents = this.owner as IThingHolderEvents<T>;
			if (thingHolderEvents != null)
			{
				thingHolderEvents.Notify_ItemRemoved(item as T);
			}
			base.NotifyRemoved(item);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x001061F4 File Offset: 0x001043F4
		public void TryAddRangeOrTransfer(IEnumerable<T> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things == this)
			{
				return;
			}
			ThingOwner thingOwner = things as ThingOwner;
			if (thingOwner != null)
			{
				thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				if (destroyLeftover)
				{
					thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				IList<T> list = things as IList<!0>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!base.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (T t in things)
				{
					if (!base.TryAddOrTransfer(t, canMergeWithExistingStacks) && destroyLeftover)
					{
						t.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x001062C8 File Offset: 0x001044C8
		public override int IndexOf(Thing item)
		{
			T t = item as T;
			if (t == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(t);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x001062F8 File Offset: 0x001044F8
		public override bool Remove(Thing item)
		{
			if (!base.Contains(item))
			{
				return false;
			}
			if (item.holdingOwner == this)
			{
				item.holdingOwner = null;
			}
			int index = this.innerList.LastIndexOf((T)((object)item));
			this.innerList.RemoveAt(index);
			this.NotifyRemoved(item);
			return true;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x00106348 File Offset: 0x00104548
		public int RemoveAll(Predicate<T> predicate)
		{
			int num = 0;
			for (int i = this.innerList.Count - 1; i >= 0; i--)
			{
				if (predicate(this.innerList[i]))
				{
					this.Remove(this.innerList[i]);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x001063A0 File Offset: 0x001045A0
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x001063B4 File Offset: 0x001045B4
		public void GetThingsOfType<J>(List<J> list) where J : Thing
		{
			for (int i = 0; i < this.innerList.Count; i++)
			{
				J j = this.innerList[i] as J;
				if (j != null)
				{
					list.Add(j);
				}
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x00106404 File Offset: 0x00104604
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x0010642B File Offset: 0x0010462B
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0010643A File Offset: 0x0010463A
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x00106448 File Offset: 0x00104648
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2, nearPlaceValidator);
			resultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0010649C File Offset: 0x0010469C
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, mode, out thing2, placedAction2, nearPlaceValidator);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x001064E8 File Offset: 0x001046E8
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator, true);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x00106538 File Offset: 0x00104738
		int IList<!0>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x00106546 File Offset: 0x00104746
		void IList<!0>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x00106552 File Offset: 0x00104752
		void ICollection<!0>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x00106562 File Offset: 0x00104762
		void ICollection<!0>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x00106571 File Offset: 0x00104771
		bool ICollection<!0>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0010657F File Offset: 0x0010477F
		bool ICollection<!0>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x0010658D File Offset: 0x0010478D
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0010659F File Offset: 0x0010479F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x04001D76 RID: 7542
		private List<T> innerList = new List<T>();
	}
}
