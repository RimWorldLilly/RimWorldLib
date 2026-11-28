using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F50 RID: 3920
	public class EntityCodexEntryDef : Def
	{
		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060064E2 RID: 25826 RVA: 0x002400F6 File Offset: 0x0023E2F6
		public string Description
		{
			get
			{
				if (this.useDescriptionFrom == null)
				{
					return this.description;
				}
				return this.useDescriptionFrom.description;
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060064E3 RID: 25827 RVA: 0x00240112 File Offset: 0x0023E312
		public bool Discovered
		{
			get
			{
				return Find.EntityCodex.Discovered(this);
			}
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x060064E4 RID: 25828 RVA: 0x0024011F File Offset: 0x0023E31F
		public bool Visible
		{
			get
			{
				return Current.ProgramState != ProgramState.Playing || !this.hideInPlaystyles.Contains(Find.Storyteller.difficulty.AnomalyPlaystyleDef);
			}
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x00240148 File Offset: 0x0023E348
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.icon = (this.uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(this.uiIconPath, true));
				this.silhouette = (this.uiIconPath.NullOrEmpty() ? BaseContent.BadTex : ContentFinder<Texture2D>.Get(this.uiIconPath + "_Silhouette", true));
			});
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x0024015C File Offset: 0x0023E35C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (ThingDef thingDef in this.linkedThings)
			{
				if (thingDef.entityCodexEntry == null)
				{
					thingDef.entityCodexEntry = this;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 4);
					defaultInterpolatedStringHandler.AppendLiteral("EntityCodexEntryDef ");
					defaultInterpolatedStringHandler.AppendFormatted(this.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" is linked to ");
					defaultInterpolatedStringHandler.AppendFormatted(thingDef.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" but ");
					defaultInterpolatedStringHandler.AppendFormatted(thingDef.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" is already linked to ");
					defaultInterpolatedStringHandler.AppendFormatted(thingDef.entityCodexEntry.defName);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			foreach (IncidentDef incidentDef in this.linkedIncidents)
			{
				if (incidentDef.codexEntry == null)
				{
					incidentDef.codexEntry = this;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(62, 4);
					defaultInterpolatedStringHandler2.AppendLiteral("EntityCodexEntryDef ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.defName);
					defaultInterpolatedStringHandler2.AppendLiteral(" is linked to ");
					defaultInterpolatedStringHandler2.AppendFormatted(incidentDef.defName);
					defaultInterpolatedStringHandler2.AppendLiteral(" but ");
					defaultInterpolatedStringHandler2.AppendFormatted(incidentDef.defName);
					defaultInterpolatedStringHandler2.AppendLiteral(" is already linked to ");
					defaultInterpolatedStringHandler2.AppendFormatted(incidentDef.codexEntry.defName);
					defaultInterpolatedStringHandler2.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x00240338 File Offset: 0x0023E538
		public override IEnumerable<string> ConfigErrors()
		{
			EntityCodexEntryDef.<ConfigErrors>d__23 <ConfigErrors>d__ = new EntityCodexEntryDef.<ConfigErrors>d__23(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003ABD RID: 15037
		public EntityCategoryDef category;

		// Token: 0x04003ABE RID: 15038
		public bool startDiscovered;

		// Token: 0x04003ABF RID: 15039
		public List<ThingDef> linkedThings = new List<ThingDef>();

		// Token: 0x04003AC0 RID: 15040
		private List<IncidentDef> linkedIncidents = new List<IncidentDef>();

		// Token: 0x04003AC1 RID: 15041
		public List<ResearchProjectDef> discoveredResearchProjects = new List<ResearchProjectDef>();

		// Token: 0x04003AC2 RID: 15042
		public List<IncidentDef> provocationIncidents = new List<IncidentDef>();

		// Token: 0x04003AC3 RID: 15043
		public EntityDiscoveryType discoveryType;

		// Token: 0x04003AC4 RID: 15044
		public bool allowDiscoveryWhileMapGenerating;

		// Token: 0x04003AC5 RID: 15045
		public int orderInCategory = 9999999;

		// Token: 0x04003AC6 RID: 15046
		public List<AnomalyPlaystyleDef> hideInPlaystyles = new List<AnomalyPlaystyleDef>();

		// Token: 0x04003AC7 RID: 15047
		private ThingDef useDescriptionFrom;

		// Token: 0x04003AC8 RID: 15048
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x04003AC9 RID: 15049
		public Texture2D icon;

		// Token: 0x04003ACA RID: 15050
		public Texture2D silhouette;

		// Token: 0x04003ACB RID: 15051
		private const string SilhouetteTexPathSuffix = "_Silhouette";
	}
}
