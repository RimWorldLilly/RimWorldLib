using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RimWorld
{
	// Token: 0x020011C8 RID: 4552
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x060071C6 RID: 29126 RVA: 0x0027E30D File Offset: 0x0027C50D
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x060071C7 RID: 29127 RVA: 0x0027E31A File Offset: 0x0027C51A
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_CategoryMTB.<MakeIntervalIncidents>d__2 <MakeIntervalIncidents>d__ = new StorytellerComp_CategoryMTB.<MakeIntervalIncidents>d__2(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x0027E334 File Offset: 0x0027C534
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(base.ToString());
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<IncidentCategoryDef>(this.Props.category);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
