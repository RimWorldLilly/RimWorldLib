using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B4 RID: 948
	[StaticConstructorOnStartup]
	public class PawnRenderNodeProperties
	{
		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x000AFB70 File Offset: 0x000ADD70
		public PawnRenderNodeWorker Worker
		{
			get
			{
				return GenWorker<PawnRenderNodeWorker>.Get(this.workerClass);
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000AFB80 File Offset: 0x000ADD80
		public List<PawnRenderSubWorker> SubWorkers
		{
			get
			{
				List<PawnRenderSubWorker> result;
				if ((result = this.subWorkersInt) == null)
				{
					result = (this.subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(this.subworkerClasses));
				}
				return result;
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000AFBAB File Offset: 0x000ADDAB
		public void EnsureInitialized()
		{
			if (this.workerInt == null)
			{
				this.workerInt = GenWorker<PawnRenderNodeWorker>.Get(this.workerClass);
			}
			if (this.subWorkersInt == null)
			{
				this.subWorkersInt = PawnRenderNodeWorker.GetSubWorkerList(this.subworkerClasses);
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000AFBDF File Offset: 0x000ADDDF
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000AFBE4 File Offset: 0x000ADDE4
		public virtual void ResolveReferencesRecursive()
		{
			this.ResolveReferences();
			if (this.children != null)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.children)
				{
					pawnRenderNodeProperties.ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000AFC44 File Offset: 0x000ADE44
		public virtual IEnumerable<string> ConfigErrors()
		{
			PawnRenderNodeProperties.<ConfigErrors>d__49 <ConfigErrors>d__ = new PawnRenderNodeProperties.<ConfigErrors>d__49(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001728 RID: 5928
		public Type workerClass = typeof(PawnRenderNodeWorker);

		// Token: 0x04001729 RID: 5929
		public List<Type> subworkerClasses;

		// Token: 0x0400172A RID: 5930
		public Type nodeClass = typeof(PawnRenderNode);

		// Token: 0x0400172B RID: 5931
		public PawnRenderNodeTagDef tagDef;

		// Token: 0x0400172C RID: 5932
		public PawnRenderNodeTagDef parentTagDef;

		// Token: 0x0400172D RID: 5933
		public List<PawnRenderNodeProperties> children;

		// Token: 0x0400172E RID: 5934
		[NoTranslate]
		public string texPath;

		// Token: 0x0400172F RID: 5935
		[NoTranslate]
		public List<string> texPaths;

		// Token: 0x04001730 RID: 5936
		[NoTranslate]
		public string texPathFemale;

		// Token: 0x04001731 RID: 5937
		[NoTranslate]
		public List<string> texPathsFemale;

		// Token: 0x04001732 RID: 5938
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths;

		// Token: 0x04001733 RID: 5939
		public int texSeed;

		// Token: 0x04001734 RID: 5940
		public bool useGraphic = true;

		// Token: 0x04001735 RID: 5941
		public RenderSkipFlagDef skipFlag;

		// Token: 0x04001736 RID: 5942
		public RotDrawMode rotDrawMode = RotDrawMode.Fresh | RotDrawMode.Rotting | RotDrawMode.Dessicated;

		// Token: 0x04001737 RID: 5943
		public PawnRenderNodeProperties.RenderNodePawnType pawnType = PawnRenderNodeProperties.RenderNodePawnType.HumanlikeOnly;

		// Token: 0x04001738 RID: 5944
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x04001739 RID: 5945
		public Color? color;

		// Token: 0x0400173A RID: 5946
		public float colorRGBPostFactor = 1f;

		// Token: 0x0400173B RID: 5947
		public bool useRottenColor;

		// Token: 0x0400173C RID: 5948
		public bool useSkinShader;

		// Token: 0x0400173D RID: 5949
		public ShaderTypeDef shaderTypeDef;

		// Token: 0x0400173E RID: 5950
		public PawnRenderNodeProperties.AttachmentColorType colorType;

		// Token: 0x0400173F RID: 5951
		public float baseLayer;

		// Token: 0x04001740 RID: 5952
		public DrawData drawData;

		// Token: 0x04001741 RID: 5953
		public bool oppositeFacingLayerWhenFlipped;

		// Token: 0x04001742 RID: 5954
		public Vector2? overrideMeshSize;

		// Token: 0x04001743 RID: 5955
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04001744 RID: 5956
		[NoTranslate]
		public string anchorTag;

		// Token: 0x04001745 RID: 5957
		public PawnRenderNodeProperties.Side side;

		// Token: 0x04001746 RID: 5958
		public float narrowCrownHorizontalOffset;

		// Token: 0x04001747 RID: 5959
		public List<Rot4> visibleFacing;

		// Token: 0x04001748 RID: 5960
		public bool flipGraphic;

		// Token: 0x04001749 RID: 5961
		public bool rotateIndependently;

		// Token: 0x0400174A RID: 5962
		public string debugLabel;

		// Token: 0x0400174B RID: 5963
		public PawnOverlayDrawer.OverlayLayer overlayLayer;

		// Token: 0x0400174C RID: 5964
		public bool? overlayOverApparel;

		// Token: 0x0400174D RID: 5965
		private PawnRenderNodeWorker workerInt;

		// Token: 0x0400174E RID: 5966
		private List<PawnRenderSubWorker> subWorkersInt;

		// Token: 0x020026B0 RID: 9904
		public enum Side
		{
			// Token: 0x04009E9C RID: 40604
			Center,
			// Token: 0x04009E9D RID: 40605
			Left,
			// Token: 0x04009E9E RID: 40606
			Right
		}

		// Token: 0x020026B1 RID: 9905
		public enum AttachmentColorType
		{
			// Token: 0x04009EA0 RID: 40608
			Custom,
			// Token: 0x04009EA1 RID: 40609
			Hair,
			// Token: 0x04009EA2 RID: 40610
			Skin
		}

		// Token: 0x020026B2 RID: 9906
		public enum RenderNodePawnType
		{
			// Token: 0x04009EA4 RID: 40612
			Any,
			// Token: 0x04009EA5 RID: 40613
			HumanlikeOnly,
			// Token: 0x04009EA6 RID: 40614
			NonHumanlikeOnly
		}
	}
}
