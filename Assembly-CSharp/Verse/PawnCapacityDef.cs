using System;

namespace Verse
{
	// Token: 0x0200019B RID: 411
	public class PawnCapacityDef : Def
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0003D94A File Offset: 0x0003BB4A
		public PawnCapacityWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnCapacityWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0003D970 File Offset: 0x0003BB70
		public string GetLabelFor(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return this.label;
			}
			if (pawn.RaceProps.IsFlesh && !this.labelAnimals.NullOrEmpty())
			{
				return this.labelAnimals;
			}
			if (pawn.RaceProps.IsAnomalyEntity && !this.labelAnomalyEntity.NullOrEmpty())
			{
				return this.labelAnomalyEntity;
			}
			if (pawn.RaceProps.IsMechanoid && !this.labelMechanoids.NullOrEmpty())
			{
				return this.labelMechanoids;
			}
			if (pawn.RaceProps.IsDrone && !this.labelDrones.NullOrEmpty())
			{
				return this.labelDrones;
			}
			return this.label;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003DA1B File Offset: 0x0003BC1B
		public string GetLabelFor()
		{
			return this.label;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003DA24 File Offset: 0x0003BC24
		public bool CanShowOnPawn(Pawn p)
		{
			if (p.def.race.Humanlike)
			{
				return this.showOnHumanlikes;
			}
			if (p.def.race.Animal)
			{
				return this.showOnAnimals;
			}
			if (p.def.race.IsAnomalyEntity)
			{
				return this.showOnAnomalyEntities;
			}
			if (p.def.race.IsDrone)
			{
				return this.showOnDrones;
			}
			return this.showOnMechanoids;
		}

		// Token: 0x040009EF RID: 2543
		public int listOrder;

		// Token: 0x040009F0 RID: 2544
		public Type workerClass = typeof(PawnCapacityWorker);

		// Token: 0x040009F1 RID: 2545
		[MustTranslate]
		public string labelMechanoids = "";

		// Token: 0x040009F2 RID: 2546
		[MustTranslate]
		public string labelAnimals = "";

		// Token: 0x040009F3 RID: 2547
		[MustTranslate]
		public string labelAnomalyEntity = "";

		// Token: 0x040009F4 RID: 2548
		[MustTranslate]
		public string labelDrones = "";

		// Token: 0x040009F5 RID: 2549
		public bool showOnHumanlikes = true;

		// Token: 0x040009F6 RID: 2550
		public bool showOnAnimals = true;

		// Token: 0x040009F7 RID: 2551
		public bool showOnMechanoids = true;

		// Token: 0x040009F8 RID: 2552
		public bool showOnAnomalyEntities = true;

		// Token: 0x040009F9 RID: 2553
		public bool showOnDrones = true;

		// Token: 0x040009FA RID: 2554
		public bool lethalFlesh;

		// Token: 0x040009FB RID: 2555
		public bool lethalMechanoids;

		// Token: 0x040009FC RID: 2556
		public float minForCapable;

		// Token: 0x040009FD RID: 2557
		public float minValue;

		// Token: 0x040009FE RID: 2558
		public bool zeroIfCannotBeAwake;

		// Token: 0x040009FF RID: 2559
		public bool showOnCaravanHealthTab;

		// Token: 0x04000A00 RID: 2560
		[Unsaved(false)]
		private PawnCapacityWorker workerInt;
	}
}
