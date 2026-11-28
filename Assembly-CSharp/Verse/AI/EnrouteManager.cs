using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000813 RID: 2067
	public class EnrouteManager : IExposable
	{
		// Token: 0x0600410C RID: 16652 RVA: 0x0019D49C File Offset: 0x0019B69C
		public EnrouteManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x0019D4C1 File Offset: 0x0019B6C1
		public void AddEnroute(IHaulEnroute container, Pawn pawn, ThingDef stuff, int count)
		{
			this.GetOrAddTracker(container).Add(pawn, stuff, count);
			pawn.MapHeld.events.Notify_HaulEnrouteAdded((Thing)container, pawn, stuff, count);
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0019D4F0 File Offset: 0x0019B6F0
		public int GetEnroute(IHaulEnroute thing, ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			ThingCountTracker thingCountTracker;
			if (this.lookup.TryGetValue(thing, out thingCountTracker))
			{
				return thingCountTracker.Get(stuff, excludeEnrouteFor);
			}
			return 0;
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0019D518 File Offset: 0x0019B718
		public bool AnyEnrouteTo(Thing thing)
		{
			IHaulEnroute haulEnroute = thing as IHaulEnroute;
			return haulEnroute != null && this.lookup.ContainsKey(haulEnroute);
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0019D540 File Offset: 0x0019B740
		public void Notify_ContainerDespawned(IHaulEnroute thing)
		{
			ThingCountTracker item;
			if (!this.lookup.TryGetValue(thing, out item))
			{
				return;
			}
			this.enroute.Remove(item);
			this.lookup.Remove(thing);
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x0019D578 File Offset: 0x0019B778
		private ThingCountTracker GetOrAddTracker(IHaulEnroute container)
		{
			ThingCountTracker thingCountTracker;
			if (this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return thingCountTracker;
			}
			thingCountTracker = new ThingCountTracker(container);
			this.enroute.Add(thingCountTracker);
			this.lookup[container] = thingCountTracker;
			return thingCountTracker;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x0019D5B8 File Offset: 0x0019B7B8
		public void SendReservations(IHaulEnroute from, IHaulEnroute to)
		{
			ThingCountTracker other;
			if (!this.lookup.TryGetValue(from, out other))
			{
				return;
			}
			this.GetOrAddTracker(to).CopyReservations(other);
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0019D5E4 File Offset: 0x0019B7E4
		public void ReleaseFor(IHaulEnroute container, Pawn pawn)
		{
			ThingCountTracker thingCountTracker;
			if (!this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return;
			}
			thingCountTracker.ReleaseFor(pawn);
			if (thingCountTracker.CanCleanup())
			{
				this.lookup.Remove(container);
				this.enroute.Remove(thingCountTracker);
			}
			this.map.events.Notify_HaulEnrouteReleased((Thing)container, pawn);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0019D644 File Offset: 0x0019B844
		public void ReleaseAllClaimedBy(Pawn pawn)
		{
			for (int i = this.enroute.Count - 1; i >= 0; i--)
			{
				ThingCountTracker thingCountTracker = this.enroute[i];
				thingCountTracker.ReleaseFor(pawn);
				if (thingCountTracker.CanCleanup())
				{
					this.lookup.Remove(thingCountTracker.parent);
					this.enroute.RemoveAt(i);
				}
				this.map.events.Notify_HaulEnrouteReleased(thingCountTracker.ParentThing, pawn);
			}
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0019D6BC File Offset: 0x0019B8BC
		public void InterruptEnroutePawns(IHaulEnroute container, Pawn exclude)
		{
			ThingCountTracker thingCountTracker;
			if (!this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return;
			}
			thingCountTracker.InterruptEnroutePawns(exclude);
			if (thingCountTracker.CanCleanup())
			{
				this.lookup.Remove(thingCountTracker.parent);
				this.enroute.Remove(thingCountTracker);
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x0019D708 File Offset: 0x0019B908
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingCountTracker>(ref this.enroute, "enroute", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.enroute.Count - 1; i >= 0; i--)
				{
					if (this.enroute[i].parent == null)
					{
						this.enroute.RemoveAt(i);
					}
				}
				foreach (ThingCountTracker thingCountTracker in this.enroute)
				{
					this.lookup.Add(thingCountTracker.parent, thingCountTracker);
				}
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0019D7BC File Offset: 0x0019B9BC
		public void LogEnroute()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Enroute =======");
			stringBuilder.AppendLine("Count: " + this.enroute.Count.ToString());
			for (int i = 0; i < this.enroute.Count; i++)
			{
				ThingCountTracker thingCountTracker = this.enroute[i];
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]: ");
				Thing thing = thingCountTracker.parent as Thing;
				defaultInterpolatedStringHandler.AppendFormatted((thing != null) ? thing.LabelShort : "NON-THING");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				if (thingCountTracker.ReadOnlyPairs.Count == 0)
				{
					stringBuilder.AppendLine("   NO ELEMENTS - SHOULD HAVE BEEN RELEASED");
				}
				else
				{
					foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in thingCountTracker.ReadOnlyPairs)
					{
						ThingDef thingDef;
						List<ThingCountTracker.PawnCount> list;
						keyValuePair.Deconstruct(out thingDef, out list);
						ThingDef thingDef2 = thingDef;
						List<ThingCountTracker.PawnCount> list2 = list;
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(14, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("   ");
						defaultInterpolatedStringHandler2.AppendFormatted(thingDef2.label);
						defaultInterpolatedStringHandler2.AppendLiteral(", ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(thingCountTracker.Get(thingDef2, null));
						defaultInterpolatedStringHandler2.AppendLiteral(" enroute:");
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
						for (int j = 0; j < list2.Count; j++)
						{
							ThingCountTracker.PawnCount pawnCount = list2[j];
							StringBuilder stringBuilder4 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(13, 3);
							defaultInterpolatedStringHandler3.AppendLiteral("      [");
							defaultInterpolatedStringHandler3.AppendFormatted<int>(j);
							defaultInterpolatedStringHandler3.AppendLiteral("]: ");
							defaultInterpolatedStringHandler3.AppendFormatted<string>(pawnCount.pawn.LabelShort, -12);
							defaultInterpolatedStringHandler3.AppendLiteral(" x ");
							defaultInterpolatedStringHandler3.AppendFormatted<int>(pawnCount.count);
							stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002B0E RID: 11022
		private List<ThingCountTracker> enroute = new List<ThingCountTracker>();

		// Token: 0x04002B0F RID: 11023
		private Map map;

		// Token: 0x04002B10 RID: 11024
		private readonly Dictionary<IHaulEnroute, ThingCountTracker> lookup = new Dictionary<IHaulEnroute, ThingCountTracker>();
	}
}
