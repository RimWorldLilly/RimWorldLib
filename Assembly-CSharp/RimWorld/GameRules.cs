using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001027 RID: 4135
	public class GameRules : IExposable
	{
		// Token: 0x06006829 RID: 26665 RVA: 0x0024D71C File Offset: 0x0024B91C
		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x0024D76F File Offset: 0x0024B96F
		public void SetAllowBuilding(ThingDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0024D7B0 File Offset: 0x0024B9B0
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			using (HashSet<Type>.Enumerator enumerator = this.disallowedDesignatorTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsAssignableFrom(d.GetType()))
					{
						return false;
					}
				}
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x0024D844 File Offset: 0x0024BA44
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		// Token: 0x0400425C RID: 16988
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		// Token: 0x0400425D RID: 16989
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
