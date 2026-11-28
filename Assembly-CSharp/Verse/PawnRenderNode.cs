using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B3 RID: 947
	public class PawnRenderNode
	{
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000AEB33 File Offset: 0x000ACD33
		public PawnRenderNodeProperties Props
		{
			get
			{
				return this.props;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x000AEB3B File Offset: 0x000ACD3B
		public virtual Graphic PrimaryGraphic
		{
			get
			{
				return this.primaryGraphic;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x000AEB43 File Offset: 0x000ACD43
		public virtual IReadOnlyList<Graphic> Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x000AEB4B File Offset: 0x000ACD4B
		public PawnRenderNodeWorker Worker
		{
			get
			{
				PawnRenderNodeProperties pawnRenderNodeProperties = this.props;
				if (pawnRenderNodeProperties == null)
				{
					return null;
				}
				return pawnRenderNodeProperties.Worker;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x000AEB5E File Offset: 0x000ACD5E
		public MaterialPropertyBlock MatPropBlock
		{
			get
			{
				return this.matPropBlock;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x000AEB66 File Offset: 0x000ACD66
		public bool DebugEnabled
		{
			get
			{
				return this.debugEnabled || !Prefs.DevMode;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x000AEB7A File Offset: 0x000ACD7A
		public Vector3 DebugOffset
		{
			get
			{
				if (!Prefs.DevMode)
				{
					return Vector3.zero;
				}
				return this.debugOffset;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x000AEB8F File Offset: 0x000ACD8F
		public float DebugAngleOffset
		{
			get
			{
				if (!Prefs.DevMode)
				{
					return 0f;
				}
				return this.debugAngleOffset;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x000AEBA4 File Offset: 0x000ACDA4
		protected virtual bool EnsureInitializationWithoutRecache
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x000AEBA7 File Offset: 0x000ACDA7
		protected virtual Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x000AEBAE File Offset: 0x000ACDAE
		public bool CheckMaterialEveryDrawRequest
		{
			get
			{
				return this.graphics.Count > 1;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x000AEBC0 File Offset: 0x000ACDC0
		public virtual bool RecacheRequested
		{
			get
			{
				if (this.requestRecache)
				{
					return true;
				}
				if (this.children != null)
				{
					for (int i = 0; i < this.children.Length; i++)
					{
						if (this.children[i].RecacheRequested)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x000AEC04 File Offset: 0x000ACE04
		public BaseAnimationWorker AnimationWorker
		{
			get
			{
				if (this.cachedAnimationWorker != null && this.cachedAnimationDef == this.tree.currentAnimation)
				{
					return this.cachedAnimationWorker;
				}
				this.cachedAnimationDef = this.tree.currentAnimation;
				this.cachedAnimationWorkerData = null;
				AnimationPart animationPart;
				if (!this.tree.TryGetAnimationPartForNode(this, out animationPart))
				{
					this.cachedAnimationWorker = null;
				}
				else
				{
					this.cachedAnimationWorker = GenWorker<BaseAnimationWorker>.Get(animationPart.WorkerType);
					this.cachedAnimationPart = animationPart;
				}
				return this.cachedAnimationWorker;
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000AEC84 File Offset: 0x000ACE84
		public PawnRenderNode(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
		{
			this.props = props;
			this.tree = tree;
			this.meshSet = this.MeshSetFor(pawn);
			try
			{
				this.Init(pawn);
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when initializing node ");
				defaultInterpolatedStringHandler.AppendFormatted<PawnRenderNode>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" for pawn ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000AED54 File Offset: 0x000ACF54
		private void Init(Pawn pawn)
		{
			if (this.props.tagDef != null)
			{
				pawn.Drawer.renderer.renderTree.SetTagNode(this.props.tagDef, this);
			}
			this.matPropBlock = new MaterialPropertyBlock();
			if (!this.props.children.NullOrEmpty<PawnRenderNodeProperties>())
			{
				this.children = new PawnRenderNode[this.props.children.Count];
				for (int i = 0; i < this.props.children.Count; i++)
				{
					PawnRenderNodeProperties pawnRenderNodeProperties = this.props.children[i];
					this.children[i] = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
					{
						pawn,
						pawnRenderNodeProperties,
						this.tree
					});
					this.children[i].parent = this;
				}
			}
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000AEE34 File Offset: 0x000AD034
		public void AppendRequests(PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			if (!this.Props.Worker.CanDrawNow(this, parms))
			{
				return;
			}
			using (List<PawnRenderSubWorker>.Enumerator enumerator = this.Props.SubWorkers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanDrawNowSub(this, parms))
					{
						return;
					}
				}
			}
			int count = requests.Count;
			this.Props.Worker.AppendDrawRequests(this, parms, requests);
			bool flag = requests.Count > count;
			if ((!this.Props.useGraphic || flag) && !this.children.NullOrEmpty<PawnRenderNode>())
			{
				for (int i = 0; i < this.children.Length; i++)
				{
					if (this.children[i].Worker.CanDrawNow(this.children[i], parms))
					{
						this.children[i].AppendRequests(parms, requests);
					}
				}
			}
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000AEF2C File Offset: 0x000AD12C
		public void EnsureInitialized(PawnRenderFlags defaultRenderFlagsNow)
		{
			if (!this.EnsureInitializationWithoutRecache && this.initialized && !this.RecacheRequested)
			{
				return;
			}
			this.props.EnsureInitialized();
			this.graphics.Clear();
			this.primaryGraphic = null;
			foreach (Graphic item in this.GraphicsFor(this.tree.pawn))
			{
				if (this.primaryGraphic == null)
				{
					this.primaryGraphic = item;
				}
				this.graphics.Add(item);
			}
			foreach (ValueTuple<GraphicStateDef, Graphic> valueTuple in this.StateGraphicsFor(this.tree.pawn))
			{
				GraphicStateDef item2 = valueTuple.Item1;
				Graphic item3 = valueTuple.Item2;
				if (this.graphicStateLookup == null)
				{
					this.graphicStateLookup = new Dictionary<GraphicStateDef, Graphic>();
				}
				if (this.primaryGraphic == null)
				{
					this.primaryGraphic = item3;
				}
				Graphic graphic;
				Graphic graphic2;
				if (!this.graphicStateLookup.TryGetValue(item2, out graphic) || !item2.TryGetDefaultGraphic(out graphic2) || graphic == graphic2)
				{
					if (graphic != null)
					{
						this.graphics.Remove(graphic);
					}
					this.graphicStateLookup[item2] = item3;
					if (!this.graphics.Contains(item3))
					{
						this.graphics.Add(item3);
					}
				}
			}
			for (int i = 0; i < this.graphics.Count; i++)
			{
				if (this.graphics[i] != null)
				{
					this.EnsureMaterialVariantsInitialized(this.graphics[i]);
				}
			}
			if (!this.meshesInitialized)
			{
				this.EnsureMeshesInitialized();
			}
			if (!this.children.NullOrEmpty<PawnRenderNode>())
			{
				for (int j = 0; j < this.children.Length; j++)
				{
					if (this.children[j] == null)
					{
						Log.Warning("Tried to ensure initialized null PawnRenderNode.");
					}
					else
					{
						this.children[j].EnsureInitialized(defaultRenderFlagsNow);
					}
				}
			}
			this.initialized = true;
			this.meshesInitialized = true;
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000AF144 File Offset: 0x000AD344
		protected virtual void EnsureMaterialVariantsInitialized(Graphic g)
		{
			this.InitializeInvisibleMaterialVariant(g);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000AF150 File Offset: 0x000AD350
		private void InitializeInvisibleMaterialVariant(Graphic g)
		{
			foreach (Rot4 facing in Rot4.AllRotations)
			{
				Material material = g.NodeGetMat(new PawnDrawParms
				{
					facing = facing,
					pawn = this.tree.pawn
				});
				if (material != null)
				{
					InvisibilityMatPool.GetInvisibleMat(material);
				}
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000AF1D0 File Offset: 0x000AD3D0
		protected virtual void EnsureMeshesInitialized()
		{
			if (this.meshSet == null)
			{
				return;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				Vector2 size = MeshPool.GetMetaData(this.meshSet.MeshAt(rot)).size;
				MeshPool.GridPlane(size);
				MeshPool.GridPlaneFlip(size);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000AF244 File Offset: 0x000AD444
		public void GetTransform(PawnDrawParms parms, out Vector3 offset, out Vector3 pivot, out Quaternion rotation, out Vector3 scale)
		{
			offset = this.Worker.OffsetFor(this, parms, out pivot);
			rotation = this.Worker.RotationFor(this, parms);
			scale = this.Worker.ScaleFor(this, parms);
			foreach (PawnRenderSubWorker pawnRenderSubWorker in this.Props.SubWorkers)
			{
				pawnRenderSubWorker.TransformOffset(this, parms, ref offset, ref pivot);
				pawnRenderSubWorker.TransformRotation(this, parms, ref rotation);
				pawnRenderSubWorker.TransformScale(this, parms, ref scale);
			}
			scale.y = 1f;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000AF2FC File Offset: 0x000AD4FC
		public virtual GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(this.props.overrideMeshSize.Value.x, this.props.overrideMeshSize.Value.y);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000AF35C File Offset: 0x000AD55C
		public virtual Graphic GraphicForState(GraphicStateDef state)
		{
			Graphic graphic = null;
			Dictionary<GraphicStateDef, Graphic> dictionary = this.graphicStateLookup;
			if (dictionary != null)
			{
				dictionary.TryGetValue(state, out graphic);
			}
			if (graphic == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(116, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Trying to query graphic state ");
				defaultInterpolatedStringHandler.AppendFormatted(state.ToStringSafe<GraphicStateDef>());
				defaultInterpolatedStringHandler.AppendLiteral(" on node ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ToStringSafe<PawnRenderNode>());
				defaultInterpolatedStringHandler.AppendLiteral(" for pawn ");
				defaultInterpolatedStringHandler.AppendFormatted(this.tree.pawn.LabelShort);
				defaultInterpolatedStringHandler.AppendLiteral(", but no valid graphic can be found. Defaulting to primary graphic.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), Gen.HashCombine<int>(this.tree.pawn.thingIDNumber, 694623110));
				graphic = this.PrimaryGraphic;
			}
			return graphic;
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000AF420 File Offset: 0x000AD620
		public virtual Graphic GraphicFor(Pawn pawn)
		{
			string text = this.TexPathFor(pawn);
			if (text.NullOrEmpty())
			{
				return null;
			}
			Shader shader = this.ShaderFor(pawn);
			if (shader == null)
			{
				return null;
			}
			return GraphicDatabase.Get<Graphic_Multi>(text, shader, Vector2.one, this.ColorFor(pawn));
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000AF465 File Offset: 0x000AD665
		protected virtual IEnumerable<Graphic> GraphicsFor(Pawn pawn)
		{
			PawnRenderNode.<GraphicsFor>d__69 <GraphicsFor>d__ = new PawnRenderNode.<GraphicsFor>d__69(-2);
			<GraphicsFor>d__.<>4__this = this;
			<GraphicsFor>d__.<>3__pawn = pawn;
			return <GraphicsFor>d__;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000AF47C File Offset: 0x000AD67C
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		protected virtual IEnumerable<ValueTuple<GraphicStateDef, Graphic>> StateGraphicsFor(Pawn pawn)
		{
			PawnRenderNode.<StateGraphicsFor>d__70 <StateGraphicsFor>d__ = new PawnRenderNode.<StateGraphicsFor>d__70(-2);
			<StateGraphicsFor>d__.<>4__this = this;
			<StateGraphicsFor>d__.<>3__pawn = pawn;
			return <StateGraphicsFor>d__;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000AF493 File Offset: 0x000AD693
		public bool HasGraphic(Pawn pawn)
		{
			return this.props.useGraphic && this.props.rotDrawMode.HasFlag(pawn.Drawer.renderer.CurRotDrawMode);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000AF4D4 File Offset: 0x000AD6D4
		public virtual Color ColorFor(Pawn pawn)
		{
			Color color;
			switch (this.props.colorType)
			{
			case PawnRenderNodeProperties.AttachmentColorType.Hair:
				if (pawn.story == null)
				{
					Log.ErrorOnce("Trying to set render node color to hair for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine<int>(pawn.thingIDNumber, 828310001));
					color = Color.white;
					goto IL_101;
				}
				color = pawn.story.HairColor;
				goto IL_101;
			case PawnRenderNodeProperties.AttachmentColorType.Skin:
			{
				Color? statueColor = pawn.Drawer.renderer.StatueColor;
				if (statueColor != null)
				{
					color = statueColor.Value;
					goto IL_101;
				}
				if (pawn.story == null)
				{
					Log.ErrorOnce("Trying to set render node color to skin for " + pawn.LabelShort + " without pawn story. Defaulting to white.", Gen.HashCombine<int>(pawn.thingIDNumber, 228340903));
					color = Color.white;
					goto IL_101;
				}
				color = pawn.story.SkinColor;
				goto IL_101;
			}
			}
			color = (this.props.color ?? Color.white);
			IL_101:
			color *= this.props.colorRGBPostFactor;
			if (this.props.useRottenColor && pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Rotting)
			{
				color = PawnRenderUtility.GetRottenColor(color);
			}
			return color;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000AF61C File Offset: 0x000AD81C
		public Shader ShaderFor(Pawn pawn)
		{
			ShaderTypeDef shaderTypeDef = this.props.shaderTypeDef;
			if (((shaderTypeDef != null) ? shaderTypeDef.Shader : null) != null)
			{
				return this.props.shaderTypeDef.Shader;
			}
			if (pawn.Drawer.renderer.StatueColor != null)
			{
				return this.DefaultShader;
			}
			if (this.props.useSkinShader)
			{
				Shader skinShader = ShaderUtility.GetSkinShader(pawn);
				if (skinShader != null)
				{
					return skinShader;
				}
			}
			return this.DefaultShader;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000AF6A0 File Offset: 0x000AD8A0
		protected virtual string TexPathFor(Pawn pawn)
		{
			if (this.Props.bodyTypeGraphicPaths != null)
			{
				foreach (BodyTypeGraphicData bodyTypeGraphicData in this.Props.bodyTypeGraphicPaths)
				{
					if (pawn.story.bodyType == bodyTypeGraphicData.bodyType)
					{
						return bodyTypeGraphicData.texturePath;
					}
				}
			}
			if (pawn.gender == Gender.Female)
			{
				if (!this.props.texPathsFemale.NullOrEmpty<string>())
				{
					using (new RandBlock(this.TexSeedFor(pawn)))
					{
						return this.props.texPathsFemale.RandomElement<string>();
					}
				}
				if (!this.props.texPathFemale.NullOrEmpty())
				{
					return this.props.texPathFemale;
				}
			}
			if (!this.props.texPaths.NullOrEmpty<string>())
			{
				using (new RandBlock(this.TexSeedFor(pawn)))
				{
					return this.props.texPaths.RandomElement<string>();
				}
			}
			return this.props.texPath;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000AF7EC File Offset: 0x000AD9EC
		protected virtual int TexSeedFor(Pawn pawn)
		{
			int num = this.props.texSeed;
			num += pawn.thingIDNumber;
			if (this.hediff != null)
			{
				num += this.hediff.loadID;
			}
			if (this.trait != null)
			{
				num += (int)this.trait.def.index;
			}
			if (this.gene != null)
			{
				num += this.gene.loadID;
			}
			return num;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000AF858 File Offset: 0x000ADA58
		public virtual Mesh GetMesh(PawnDrawParms parms)
		{
			if (this.meshSet == null)
			{
				return null;
			}
			Mesh mesh = this.meshSet.MeshAt(parms.facing);
			if (this.FlipGraphic(parms))
			{
				mesh = MeshPool.GridPlaneFlip(mesh);
			}
			return mesh;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000AF894 File Offset: 0x000ADA94
		public virtual bool FlipGraphic(PawnDrawParms parms)
		{
			bool flag = this.props.flipGraphic;
			if (this.debugFlip)
			{
				flag = !flag;
			}
			BodyPartRecord bodyPartRecord = this.bodyPart;
			if (bodyPartRecord != null && bodyPartRecord.flipGraphic)
			{
				flag = !flag;
			}
			if (this.Props.drawData != null && this.Props.drawData.FlipForRot(parms.facing))
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000AF900 File Offset: 0x000ADB00
		public void AddChildren(PawnRenderNode[] newChildren)
		{
			if (this.children == null)
			{
				this.children = newChildren;
				return;
			}
			PawnRenderNode[] array = new PawnRenderNode[this.children.Length + newChildren.Length];
			int i;
			for (i = 0; i < this.children.Length; i++)
			{
				array[i] = this.children[i];
			}
			for (int j = 0; j < newChildren.Length; j++)
			{
				array[i + j] = newChildren[j];
			}
			this.children = array;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000AF96C File Offset: 0x000ADB6C
		public bool TryGetAnimationOffset(PawnDrawParms parms, out Vector3 offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.cachedAnimationDef.posRotScale * this.AnimationWorker.OffsetAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = Vector3.zero;
			return false;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000AF9CC File Offset: 0x000ADBCC
		public bool TryGetAnimationScale(PawnDrawParms parms, out Vector3 offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.AnimationWorker.ScaleAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = Vector3.one;
			return false;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000AFA1C File Offset: 0x000ADC1C
		public bool TryGetAnimationRotation(PawnDrawParms parms, out float offset)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				offset = this.cachedAnimationDef.posRotScale * this.AnimationWorker.AngleAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			offset = 0f;
			return false;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000AFA6E File Offset: 0x000ADC6E
		public bool TryGetAnimationGraphicState(PawnDrawParms parms, out GraphicStateDef state)
		{
			if (this.AnimationWorkerEnabled(parms))
			{
				state = this.AnimationWorker.GraphicStateAtTick(this.tree.AnimationTick, this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
				return true;
			}
			state = null;
			return false;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000AFAA5 File Offset: 0x000ADCA5
		public bool ContainsGraphicState(GraphicStateDef state)
		{
			return this.graphicStateLookup != null && this.graphicStateLookup.ContainsKey(state);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000AFABD File Offset: 0x000ADCBD
		public void TryAnimationPostDraw(PawnDrawParms parms, Matrix4x4 matrix)
		{
			BaseAnimationWorker animationWorker = this.AnimationWorker;
			if (animationWorker != null && animationWorker.Enabled(this.cachedAnimationDef, this, this.cachedAnimationPart, parms))
			{
				this.AnimationWorker.PostDraw(this.cachedAnimationDef, this, this.cachedAnimationPart, parms, matrix);
			}
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000AFAFB File Offset: 0x000ADCFB
		public bool AnimationWorkerEnabled(PawnDrawParms parms)
		{
			BaseAnimationWorker animationWorker = this.AnimationWorker;
			return animationWorker != null && animationWorker.Enabled(this.cachedAnimationDef, this, this.cachedAnimationPart, parms);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000AFB1C File Offset: 0x000ADD1C
		public T GetAnimationWorkerData<T>() where T : new()
		{
			object obj = this.cachedAnimationWorkerData;
			if (obj is T)
			{
				return (T)((object)obj);
			}
			T t = Activator.CreateInstance<T>();
			this.cachedAnimationWorkerData = t;
			return t;
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000AFB54 File Offset: 0x000ADD54
		public override string ToString()
		{
			if (this.props != null)
			{
				return this.props.debugLabel;
			}
			return base.ToString();
		}

		// Token: 0x04001708 RID: 5896
		public PawnRenderTree tree;

		// Token: 0x04001709 RID: 5897
		protected Graphic primaryGraphic;

		// Token: 0x0400170A RID: 5898
		protected GraphicMeshSet meshSet;

		// Token: 0x0400170B RID: 5899
		protected readonly PawnRenderNodeProperties props;

		// Token: 0x0400170C RID: 5900
		protected MaterialPropertyBlock matPropBlock;

		// Token: 0x0400170D RID: 5901
		public PawnRenderNode parent;

		// Token: 0x0400170E RID: 5902
		public PawnRenderNode[] children;

		// Token: 0x0400170F RID: 5903
		public BodyPartRecord bodyPart;

		// Token: 0x04001710 RID: 5904
		public Gene gene;

		// Token: 0x04001711 RID: 5905
		public Hediff hediff;

		// Token: 0x04001712 RID: 5906
		public Trait trait;

		// Token: 0x04001713 RID: 5907
		public Apparel apparel;

		// Token: 0x04001714 RID: 5908
		public bool requestRecache;

		// Token: 0x04001715 RID: 5909
		private bool meshesInitialized;

		// Token: 0x04001716 RID: 5910
		private bool initialized;

		// Token: 0x04001717 RID: 5911
		private readonly List<Graphic> graphics = new List<Graphic>(1);

		// Token: 0x04001718 RID: 5912
		protected Dictionary<GraphicStateDef, Graphic> graphicStateLookup;

		// Token: 0x04001719 RID: 5913
		public bool debugEnabled = true;

		// Token: 0x0400171A RID: 5914
		public Vector3 debugOffset = Vector3.zero;

		// Token: 0x0400171B RID: 5915
		public float debugAngleOffset;

		// Token: 0x0400171C RID: 5916
		public float debugScale = 1f;

		// Token: 0x0400171D RID: 5917
		public float debugLayerOffset;

		// Token: 0x0400171E RID: 5918
		public bool debugFlip;

		// Token: 0x0400171F RID: 5919
		public Vector2 debugPivotOffset = DrawData.PivotCenter;

		// Token: 0x04001720 RID: 5920
		private AnimationDef cachedAnimationDef;

		// Token: 0x04001721 RID: 5921
		private BaseAnimationWorker cachedAnimationWorker;

		// Token: 0x04001722 RID: 5922
		private AnimationPart cachedAnimationPart;

		// Token: 0x04001723 RID: 5923
		public object cachedAnimationWorkerData;

		// Token: 0x04001724 RID: 5924
		private Graphic lastDrawnGraphic;

		// Token: 0x04001725 RID: 5925
		public const int MinLayer = -10;

		// Token: 0x04001726 RID: 5926
		public const int SubIntervalCount = 100;

		// Token: 0x04001727 RID: 5927
		public const float SubInterval = 0.0003658537f;
	}
}
