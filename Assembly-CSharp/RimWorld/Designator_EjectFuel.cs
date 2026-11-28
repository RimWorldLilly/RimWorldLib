using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C45 RID: 7237
	public class Designator_EjectFuel : Designator
	{
		// Token: 0x17002151 RID: 8529
		// (get) Token: 0x0600B887 RID: 47239 RVA: 0x00411A44 File Offset: 0x0040FC44
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.EjectFuel;
			}
		}

		// Token: 0x17002152 RID: 8530
		// (get) Token: 0x0600B888 RID: 47240 RVA: 0x00411A4B File Offset: 0x0040FC4B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B889 RID: 47241 RVA: 0x00411A54 File Offset: 0x0040FC54
		public Designator_EjectFuel()
		{
			this.defaultLabel = "DesignatorEjectFuel".Translate();
			this.defaultDesc = "DesignatorEjectFuelDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/EjectFuel", true);
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x0600B88A RID: 47242 RVA: 0x00411AB4 File Offset: 0x0040FCB4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.RefuelablesInCell(c).Any<Thing>())
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B88B RID: 47243 RVA: 0x00411AE8 File Offset: 0x0040FCE8
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.RefuelablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B88C RID: 47244 RVA: 0x00411B38 File Offset: 0x0040FD38
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			CompRefuelable compRefuelable;
			if (!t.TryGetComp(out compRefuelable))
			{
				return false;
			}
			AcceptanceReport result = compRefuelable.CanEjectFuel();
			if (!result.Accepted)
			{
				return result;
			}
			return base.Map.designationManager.DesignationOn(t, this.Designation) == null;
		}

		// Token: 0x0600B88D RID: 47245 RVA: 0x00411B87 File Offset: 0x0040FD87
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}

		// Token: 0x0600B88E RID: 47246 RVA: 0x00411BAB File Offset: 0x0040FDAB
		private IEnumerable<Thing> RefuelablesInCell(IntVec3 c)
		{
			Designator_EjectFuel.<RefuelablesInCell>d__9 <RefuelablesInCell>d__ = new Designator_EjectFuel.<RefuelablesInCell>d__9(-2);
			<RefuelablesInCell>d__.<>4__this = this;
			<RefuelablesInCell>d__.<>3__c = c;
			return <RefuelablesInCell>d__;
		}
	}
}
