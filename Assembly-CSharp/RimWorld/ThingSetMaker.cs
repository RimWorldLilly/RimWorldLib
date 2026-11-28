using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001000 RID: 4096
	public abstract class ThingSetMaker
	{
		// Token: 0x06006740 RID: 26432 RVA: 0x00248ECC File Offset: 0x002470CC
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06006741 RID: 26433 RVA: 0x00248EE8 File Offset: 0x002470E8
		public List<Thing> Generate(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			ThingSetMaker.thingsBeingGeneratedNow.Add(list);
			try
			{
				ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
				this.Generate(parms2, list);
				this.PostProcess(list);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating thing set: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy(DestroyMode.Vanish);
					list.RemoveAt(i);
				}
			}
			finally
			{
				ThingSetMaker.thingsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x00248F90 File Offset: 0x00247190
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x00248FAC File Offset: 0x002471AC
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06006744 RID: 26436
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06006745 RID: 26437 RVA: 0x00248FB0 File Offset: 0x002471B0
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x00248FCC File Offset: 0x002471CC
		public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
		{
			ThingSetMaker.<AllGeneratableThingsDebug>d__9 <AllGeneratableThingsDebug>d__ = new ThingSetMaker.<AllGeneratableThingsDebug>d__9(-2);
			<AllGeneratableThingsDebug>d__.<>4__this = this;
			<AllGeneratableThingsDebug>d__.<>3__parms = parms;
			return <AllGeneratableThingsDebug>d__;
		}

		// Token: 0x06006747 RID: 26439 RVA: 0x00248FE3 File Offset: 0x002471E3
		public virtual float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			return 1f;
		}

		// Token: 0x06006748 RID: 26440
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06006749 RID: 26441 RVA: 0x00248FEC File Offset: 0x002471EC
		private void PostProcess(List<Thing> things)
		{
			if (things.RemoveAll((Thing x) => x == null) != 0)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " generated null things.");
			}
			this.ChangeDeadPawnsToTheirCorpses(things);
			for (int i = things.Count - 1; i >= 0; i--)
			{
				if (things[i].Destroyed)
				{
					Type type2 = base.GetType();
					Log.Error(((type2 != null) ? type2.ToString() : null) + " generated destroyed thing " + things[i].ToStringSafe<Thing>());
					things.RemoveAt(i);
				}
				else if (things[i].stackCount <= 0)
				{
					string[] array = new string[5];
					int num = 0;
					Type type3 = base.GetType();
					array[num] = ((type3 != null) ? type3.ToString() : null);
					array[1] = " generated ";
					array[2] = things[i].ToStringSafe<Thing>();
					array[3] = " with stackCount=";
					array[4] = things[i].stackCount.ToString();
					Log.Error(string.Concat(array));
					things.RemoveAt(i);
				}
			}
			this.Minify(things);
		}

		// Token: 0x0600674A RID: 26442 RVA: 0x0024911C File Offset: 0x0024731C
		private void Minify(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.Minifiable)
				{
					int stackCount = things[i].stackCount;
					things[i].stackCount = 1;
					MinifiedThing minifiedThing = things[i].MakeMinified(DestroyMode.Vanish);
					minifiedThing.stackCount = stackCount;
					things[i] = minifiedThing;
				}
			}
		}

		// Token: 0x0600674B RID: 26443 RVA: 0x00249184 File Offset: 0x00247384
		private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].ParentHolder is Corpse)
				{
					things[i] = (Corpse)things[i].ParentHolder;
				}
			}
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x002491D0 File Offset: 0x002473D0
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x0600674D RID: 26445 RVA: 0x002491ED File Offset: 0x002473ED
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		// Token: 0x0600674E RID: 26446 RVA: 0x0024920C File Offset: 0x0024740C
		public virtual IEnumerable<string> ConfigErrors()
		{
			return new ThingSetMaker.<ConfigErrors>d__17(-2);
		}

		// Token: 0x04004117 RID: 16663
		public ThingSetMakerParams fixedParams;

		// Token: 0x04004118 RID: 16664
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();
	}
}
