using System;

namespace Verse
{
	// Token: 0x020001FD RID: 509
	public abstract class Entity
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000EA6 RID: 3750
		public abstract string LabelCap { get; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000EA7 RID: 3751
		public abstract string Label { get; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0004BE58 File Offset: 0x0004A058
		public virtual string LabelShort
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0004BE60 File Offset: 0x0004A060
		public virtual string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0004BE68 File Offset: 0x0004A068
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		// Token: 0x06000EAB RID: 3755
		public abstract void SpawnSetup(Map map, bool respawningAfterLoad);

		// Token: 0x06000EAC RID: 3756
		public abstract void DeSpawn(DestroyMode mode = DestroyMode.Vanish);

		// Token: 0x06000EAD RID: 3757 RVA: 0x0004BE75 File Offset: 0x0004A075
		protected virtual void Tick()
		{
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0004BE77 File Offset: 0x0004A077
		protected virtual void TickInterval(int delta)
		{
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0004BE79 File Offset: 0x0004A079
		public virtual void TickRare()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0004BE80 File Offset: 0x0004A080
		public virtual void TickLong()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0004BE87 File Offset: 0x0004A087
		public override string ToString()
		{
			return this.LabelCap;
		}
	}
}
