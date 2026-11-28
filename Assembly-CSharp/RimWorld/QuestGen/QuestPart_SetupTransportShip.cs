using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A0 RID: 8608
	public class QuestPart_SetupTransportShip : QuestPart
	{
		// Token: 0x0600D6B4 RID: 54964 RVA: 0x004FB87C File Offset: 0x004F9A7C
		public override bool QuestPartReserves(Pawn p)
		{
			if (p == null)
			{
				return false;
			}
			if (this.pawns.NotNullAndContains(p))
			{
				return true;
			}
			TransportShip transportShip = this.transportShip;
			bool flag;
			if (transportShip == null)
			{
				flag = (null != null);
			}
			else
			{
				CompTransporter transporterComp = transportShip.TransporterComp;
				flag = (((transporterComp != null) ? transporterComp.innerContainer : null) != null);
			}
			return flag && this.transportShip.TransporterComp.innerContainer.Contains(p);
		}

		// Token: 0x0600D6B5 RID: 54965 RVA: 0x004FB8D6 File Offset: 0x004F9AD6
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x0600D6B6 RID: 54966 RVA: 0x004FB8E1 File Offset: 0x004F9AE1
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			List<Pawn> list = this.pawns;
			if (list == null)
			{
				return;
			}
			list.Replace(replace, with);
		}

		// Token: 0x0600D6B7 RID: 54967 RVA: 0x004FB8F8 File Offset: 0x004F9AF8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsWorldPawn())
							{
								Log.Error("Trying to transfer a non-world pawn to a transportShip.");
							}
						}
					}
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.pawns, true, true);
					this.pawns.Clear();
				}
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.items, true, true);
					this.items.Clear();
				}
				this.transportShip.Start();
			}
		}

		// Token: 0x0600D6B8 RID: 54968 RVA: 0x004FB9E0 File Offset: 0x004F9BE0
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
			this.items = null;
			this.pawns = null;
		}

		// Token: 0x0600D6B9 RID: 54969 RVA: 0x004FBA00 File Offset: 0x004F9C00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.items.RemoveAll((Thing x) => x == null);
				}
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					this.pawns.RemoveAll((Pawn x) => x == null);
				}
			}
		}

		// Token: 0x04008800 RID: 34816
		public TransportShip transportShip;

		// Token: 0x04008801 RID: 34817
		public List<Pawn> pawns;

		// Token: 0x04008802 RID: 34818
		public List<Thing> items;

		// Token: 0x04008803 RID: 34819
		public string inSignal;
	}
}
