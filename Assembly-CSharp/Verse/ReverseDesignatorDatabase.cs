using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020005B2 RID: 1458
	public class ReverseDesignatorDatabase
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x0010C330 File Offset: 0x0010A530
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0010C346 File Offset: 0x0010A546
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0010C350 File Offset: 0x0010A550
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x0010C3AC File Offset: 0x0010A5AC
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_DeconstructConduit());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_PlantsHarvestWood());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_MineVein());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_Open());
			this.desList.Add(new Designator_EjectFuel());
			this.desList.Add(new Designator_SmoothSurface());
			this.desList.Add(new Designator_SmoothFloors());
			this.desList.Add(new Designator_SmoothWalls());
			this.desList.Add(new Designator_ReleaseAnimalToWild());
			this.desList.Add(new Designator_ExtractTree());
			this.desList.Add(new Designator_RemovePaint());
			if (ModsConfig.BiotechActive)
			{
				this.desList.Add(new Designator_MechControlGroup());
				this.desList.Add(new Designator_Adopt());
			}
			this.desList.Add(new Designator_FillIn());
			this.desList.Add(new Designator_ExtractSkull());
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x04001E06 RID: 7686
		private List<Designator> desList;
	}
}
