using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001015 RID: 4117
	public class WorldObjectDef : Def
	{
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x060067B5 RID: 26549 RVA: 0x0024A5E0 File Offset: 0x002487E0
		public Material Material
		{
			get
			{
				if (this.texture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					string texPath = this.texture;
					ShaderTypeDef shaderTypeDef = this.shader;
					this.material = MaterialPool.MatFrom(texPath, ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.WorldOverlayTransparentLit, 3550);
				}
				return this.material;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x060067B6 RID: 26550 RVA: 0x0024A641 File Offset: 0x00248841
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x060067B7 RID: 26551 RVA: 0x0024A678 File Offset: 0x00248878
		public Texture2D ExpandingIconTextureMask
		{
			get
			{
				if (this.expandingIconTextureMaskInt == null)
				{
					if (this.expandingIconMaskTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureMaskInt = ContentFinder<Texture2D>.Get(this.expandingIconMaskTexture, true);
				}
				return this.expandingIconTextureMaskInt;
			}
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x060067B8 RID: 26552 RVA: 0x0024A6AF File Offset: 0x002488AF
		public Material MapEdgeMaterial
		{
			get
			{
				if (this.mapEdgeTexture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.mapEdgeTexture, ShaderDatabase.MapEdgeTerrain);
				}
				return this.material;
			}
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x0024A6EC File Offset: 0x002488EC
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate inspector tab of type ";
						Type type = this.inspectorTabs[i];
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x0024A7A0 File Offset: 0x002489A0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0024A7DB File Offset: 0x002489DB
		public override IEnumerable<string> ConfigErrors()
		{
			WorldObjectDef.<ConfigErrors>d__51 <ConfigErrors>d__ = new WorldObjectDef.<ConfigErrors>d__51(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040041FA RID: 16890
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x040041FB RID: 16891
		public bool canHaveFaction = true;

		// Token: 0x040041FC RID: 16892
		public bool saved = true;

		// Token: 0x040041FD RID: 16893
		public bool canBePlayerHome;

		// Token: 0x040041FE RID: 16894
		public bool treatAsPlayerHome;

		// Token: 0x040041FF RID: 16895
		public bool canResizeToGravship;

		// Token: 0x04004200 RID: 16896
		public bool requiresSignalJammerToReach;

		// Token: 0x04004201 RID: 16897
		public IntVec3? overrideMapSize;

		// Token: 0x04004202 RID: 16898
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x04004203 RID: 16899
		public bool canHaveMap = true;

		// Token: 0x04004204 RID: 16900
		public bool validLaunchTarget = true;

		// Token: 0x04004205 RID: 16901
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x04004206 RID: 16902
		public bool isTempIncidentMapOwner;

		// Token: 0x04004207 RID: 16903
		public List<IncidentTargetTagDef> IncidentTargetTags;

		// Token: 0x04004208 RID: 16904
		public bool selectable = true;

		// Token: 0x04004209 RID: 16905
		public bool neverMultiSelect;

		// Token: 0x0400420A RID: 16906
		public MapGeneratorDef mapGenerator;

		// Token: 0x0400420B RID: 16907
		public List<Type> inspectorTabs;

		// Token: 0x0400420C RID: 16908
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x0400420D RID: 16909
		public bool useDynamicDrawer;

		// Token: 0x0400420E RID: 16910
		public bool expandingIcon;

		// Token: 0x0400420F RID: 16911
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04004210 RID: 16912
		[NoTranslate]
		public string expandingIconMaskTexture;

		// Token: 0x04004211 RID: 16913
		public float expandingIconPriority;

		// Token: 0x04004212 RID: 16914
		[NoTranslate]
		public string texture;

		// Token: 0x04004213 RID: 16915
		[NoTranslate]
		public string mapEdgeTexture;

		// Token: 0x04004214 RID: 16916
		private ShaderTypeDef mapEdgeShader;

		// Token: 0x04004215 RID: 16917
		[Unsaved(false)]
		private Material material;

		// Token: 0x04004216 RID: 16918
		[Unsaved(false)]
		private Material expandingMaterial;

		// Token: 0x04004217 RID: 16919
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04004218 RID: 16920
		[Unsaved(false)]
		private Texture2D expandingIconTextureMaskInt;

		// Token: 0x04004219 RID: 16921
		public bool expandMore;

		// Token: 0x0400421A RID: 16922
		public bool rotateGraphicWhenTraveling;

		// Token: 0x0400421B RID: 16923
		public Color? expandingIconColor;

		// Token: 0x0400421C RID: 16924
		public float expandingIconDrawSize = 1f;

		// Token: 0x0400421D RID: 16925
		public bool fullyExpandedInSpace;

		// Token: 0x0400421E RID: 16926
		public ShaderTypeDef shader;

		// Token: 0x0400421F RID: 16927
		public ShaderTypeDef expandingShader;

		// Token: 0x04004220 RID: 16928
		public float drawAltitudeOffset;

		// Token: 0x04004221 RID: 16929
		public bool blockExitGridUntilBattleIsWon;

		// Token: 0x04004222 RID: 16930
		public RulePackDef nameMaker;
	}
}
