using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002001 RID: 8193
	public abstract class RoomRequirement : IExposable
	{
		// Token: 0x0600CF90 RID: 53136 RVA: 0x004C6730 File Offset: 0x004C4930
		public bool MetOrDisabled(Room room, Pawn p = null)
		{
			return this.Disabled(room, p) || this.Met(room, p);
		}

		// Token: 0x0600CF91 RID: 53137 RVA: 0x004C6748 File Offset: 0x004C4948
		public virtual bool Disabled(Room room, Pawn p = null)
		{
			if (this.disablingPrecepts != null && p != null && p.Ideo != null)
			{
				foreach (Precept precept in p.Ideo.PreceptsListForReading)
				{
					if (this.disablingPrecepts.Contains(precept.def))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600CF92 RID: 53138
		public abstract bool Met(Room r, Pawn p = null);

		// Token: 0x0600CF93 RID: 53139 RVA: 0x004C67C8 File Offset: 0x004C49C8
		public virtual string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x0600CF94 RID: 53140 RVA: 0x004C67DA File Offset: 0x004C49DA
		public string LabelCap(Room r = null)
		{
			return this.Label(r).CapitalizeFirst();
		}

		// Token: 0x0600CF95 RID: 53141 RVA: 0x004C67E8 File Offset: 0x004C49E8
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0600CF96 RID: 53142 RVA: 0x004C67EF File Offset: 0x004C49EF
		public virtual bool SameOrSubsetOf(RoomRequirement other)
		{
			return base.GetType() == other.GetType();
		}

		// Token: 0x0600CF97 RID: 53143 RVA: 0x004C6802 File Offset: 0x004C4A02
		public virtual bool PlayerCanBuildNow()
		{
			return true;
		}

		// Token: 0x0600CF98 RID: 53144 RVA: 0x004C6805 File Offset: 0x004C4A05
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.labelKey, "labelKey", null, false);
		}

		// Token: 0x04008419 RID: 33817
		public List<PreceptDef> disablingPrecepts;

		// Token: 0x0400841A RID: 33818
		[NoTranslate]
		public string labelKey;
	}
}
