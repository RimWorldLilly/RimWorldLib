using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000392 RID: 914
	public class PawnRenderTree
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x000ABE9A File Offset: 0x000AA09A
		public bool Resolved
		{
			get
			{
				return this.rootNode != null;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x000ABEA8 File Offset: 0x000AA0A8
		public Graphic BodyGraphic
		{
			get
			{
				this.TrySetupGraphIfNeeded();
				PawnRenderNode pawnRenderNode;
				if (!this.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Body, out pawnRenderNode))
				{
					return null;
				}
				if (pawnRenderNode == null)
				{
					return null;
				}
				return pawnRenderNode.PrimaryGraphic;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x000ABEDC File Offset: 0x000AA0DC
		public Graphic HeadGraphic
		{
			get
			{
				this.TrySetupGraphIfNeeded();
				PawnRenderNode pawnRenderNode;
				if (!this.nodesByTag.TryGetValue(PawnRenderNodeTagDefOf.Head, out pawnRenderNode))
				{
					return null;
				}
				if (pawnRenderNode == null)
				{
					return null;
				}
				return pawnRenderNode.PrimaryGraphic;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x000ABF10 File Offset: 0x000AA110
		public int AnimationTick
		{
			get
			{
				if (this.currentAnimation == null || this.currentAnimation.durationTicks <= 0)
				{
					return 0;
				}
				int num = GenTicks.TicksGame - this.animationStartTick;
				if (this.currentAnimation.loopMode != LoopMode.Clamp)
				{
					return GenMath.PositiveMod(num, this.currentAnimation.durationTicks);
				}
				if (num <= this.currentAnimation.durationTicks)
				{
					return num;
				}
				return this.currentAnimation.durationTicks;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x000ABF80 File Offset: 0x000AA180
		public bool AnimationFinished
		{
			get
			{
				if (this.currentAnimation == null || this.currentAnimation.durationTicks <= 0)
				{
					return false;
				}
				int num = GenTicks.TicksGame - this.animationStartTick;
				return this.currentAnimation.loopMode == LoopMode.End && num > this.currentAnimation.durationTicks;
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000ABFD0 File Offset: 0x000AA1D0
		public PawnRenderTree(Pawn pawn)
		{
			this.pawn = pawn;
			if (PawnRenderTree.dynamicNodeTypeInstances.Count == 0)
			{
				PawnRenderTree.InitializeDynamicNodeTypes();
			}
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000AC040 File Offset: 0x000AA240
		private static void InitializeDynamicNodeTypes()
		{
			foreach (Type type in typeof(DynamicPawnRenderNodeSetup).AllSubclassesNonAbstract())
			{
				PawnRenderTree.dynamicNodeTypeInstances.Add((DynamicPawnRenderNodeSetup)Activator.CreateInstance(type));
			}
			PawnRenderTree.dynamicNodeTypeInstances.Sort(delegate(DynamicPawnRenderNodeSetup x, DynamicPawnRenderNodeSetup y)
			{
				if (x.SetupAfter != null && x.SetupAfter.Contains(y.GetType()))
				{
					return 1;
				}
				if (y.SetupAfter != null && y.SetupAfter.Contains(x.GetType()))
				{
					return -1;
				}
				return 0;
			});
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000AC0D4 File Offset: 0x000AA2D4
		public void EnsureInitialized(PawnRenderFlags defaultRenderFlagsNow)
		{
			this.TrySetupGraphIfNeeded();
			PawnRenderNode pawnRenderNode = this.rootNode;
			if (pawnRenderNode == null)
			{
				return;
			}
			pawnRenderNode.EnsureInitialized(defaultRenderFlagsNow);
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000AC0F0 File Offset: 0x000AA2F0
		public void ParallelPreDraw(PawnDrawParms parms)
		{
			if (!this.HasValidPropsAndRenderTree)
			{
				return;
			}
			this.AdjustParms(ref parms);
			if (this.oldParms.ShouldRecache(parms) || this.rootNode.RecacheRequested)
			{
				this.TraverseTree(delegate(PawnRenderNode node)
				{
					node.requestRecache = false;
				});
				this.drawRequests.Clear();
				this.rootNode.AppendRequests(parms, this.drawRequests);
				this.oldParms = parms;
			}
			for (int i = 0; i < this.drawRequests.Count; i++)
			{
				PawnGraphicDrawRequest pawnGraphicDrawRequest = this.drawRequests[i];
				Matrix4x4 preDrawnComputedMatrix;
				if (!this.TryGetMatrix(pawnGraphicDrawRequest.node, parms, out preDrawnComputedMatrix))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to get matrix for ");
					defaultInterpolatedStringHandler.AppendFormatted<PawnRenderNode>(pawnGraphicDrawRequest.node);
					defaultInterpolatedStringHandler.AppendLiteral(" on ");
					defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), Gen.HashCombine<int>(174383246, this.pawn.GetHashCode()));
					return;
				}
				if (pawnGraphicDrawRequest.node.CheckMaterialEveryDrawRequest)
				{
					pawnGraphicDrawRequest.material = pawnGraphicDrawRequest.node.Worker.GetFinalizedMaterial(pawnGraphicDrawRequest.node, parms);
				}
				pawnGraphicDrawRequest.preDrawnComputedMatrix = preDrawnComputedMatrix;
				this.drawRequests[i] = pawnGraphicDrawRequest;
			}
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000AC258 File Offset: 0x000AA458
		public void Draw(PawnDrawParms parms)
		{
			if (!this.Resolved)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to draw ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" without a resolved render tree.");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), Gen.HashCombine<int>(174383246, this.pawn.GetHashCode()));
				return;
			}
			using (new ProfilerBlock("PawnRenderTree.Draw"))
			{
				for (int i = 0; i < this.drawRequests.Count; i++)
				{
					PawnGraphicDrawRequest pawnGraphicDrawRequest = this.drawRequests[i];
					Material material = pawnGraphicDrawRequest.material;
					if (material != null)
					{
						pawnGraphicDrawRequest.node.Worker.PreDraw(pawnGraphicDrawRequest.node, material, parms);
						MaterialPropertyBlock materialPropertyBlock = pawnGraphicDrawRequest.node.Worker.GetMaterialPropertyBlock(pawnGraphicDrawRequest.node, material, parms);
						foreach (PawnRenderSubWorker pawnRenderSubWorker in pawnGraphicDrawRequest.node.Props.SubWorkers)
						{
							pawnRenderSubWorker.EditMaterialPropertyBlock(pawnGraphicDrawRequest.node, material, parms, ref materialPropertyBlock);
						}
						GenDraw.DrawMeshNowOrLater(pawnGraphicDrawRequest.mesh, pawnGraphicDrawRequest.preDrawnComputedMatrix, material, parms.DrawNow, materialPropertyBlock);
						materialPropertyBlock.Clear();
					}
					pawnGraphicDrawRequest.node.Worker.PostDraw(pawnGraphicDrawRequest.node, parms, pawnGraphicDrawRequest.mesh, pawnGraphicDrawRequest.preDrawnComputedMatrix);
				}
			}
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000AC410 File Offset: 0x000AA610
		public bool GetRootTPRS(PawnDrawParms parms, out Vector3 offset, out Vector3 pivot, out Quaternion rotation, out Vector3 scale)
		{
			offset = Vector3.zero;
			pivot = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.zero;
			if (this.rootNode == null || this.nodeAncestors == null)
			{
				return false;
			}
			List<PawnRenderNode> list;
			bool result = this.nodeAncestors.TryGetValue(this.rootNode, out list);
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 b;
				Vector3 b2;
				Quaternion rhs;
				Vector3 b3;
				list[i].GetTransform(parms, out b, out b2, out rhs, out b3);
				offset += b;
				pivot += b2;
				rotation *= rhs;
				scale += b3;
			}
			return result;
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000AC4E8 File Offset: 0x000AA6E8
		public bool TryGetMatrix(PawnRenderNode node, PawnDrawParms parms, out Matrix4x4 matrix)
		{
			matrix = parms.matrix;
			List<PawnRenderNode> list;
			if (!this.nodeAncestors.TryGetValue(node, out list))
			{
				this.SetDirty();
				this.TrySetupGraphIfNeeded();
				if (!this.nodeAncestors.TryGetValue(node, out list))
				{
					return false;
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				Vector3 vector;
				Vector3 vector2;
				Quaternion quaternion;
				Vector3 vector3;
				list[i].GetTransform(parms, out vector, out vector2, out quaternion, out vector3);
				bool canRotate = !node.Props.rotateIndependently || list[i] == node;
				this.ComputeMatrix(ref matrix, vector, vector2, quaternion, vector3, canRotate);
			}
			float num = node.Worker.AltitudeFor(node, parms);
			if (num != 0f)
			{
				matrix *= Matrix4x4.Translate(Vector3.up * num);
			}
			return true;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000AC5BC File Offset: 0x000AA7BC
		[BurstCompile]
		private void ComputeMatrix(ref Matrix4x4 matrix, in Vector3 offset, in Vector3 pivot, in Quaternion rotation, in Vector3 scale, bool canRotate)
		{
			if (offset != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(offset);
			}
			if (pivot != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(pivot);
			}
			if (canRotate && rotation != Quaternion.identity)
			{
				matrix *= Matrix4x4.Rotate(rotation);
			}
			if (scale != Vector3.one)
			{
				matrix *= Matrix4x4.Scale(scale);
			}
			if (pivot != Vector3.zero)
			{
				matrix *= Matrix4x4.Translate(pivot).inverse;
			}
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000AC6C0 File Offset: 0x000AA8C0
		private void AdjustParms(ref PawnDrawParms parms)
		{
			if (this.debugTint != null)
			{
				parms.tint *= this.debugTint.Value;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				if (parms.crawling && !parms.Portrait && parms.facing == Rot4.South)
				{
					parms.facing = Rot4.North;
					parms.flipHead = true;
				}
				if (this.pawn.apparel != null && PawnRenderNodeWorker_Apparel_Head.HeadgearVisible(parms))
				{
					foreach (Apparel apparel in this.pawn.apparel.WornApparel)
					{
						if (apparel.def.apparel.renderSkipFlags != null)
						{
							using (List<RenderSkipFlagDef>.Enumerator enumerator2 = apparel.def.apparel.renderSkipFlags.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									RenderSkipFlagDef renderSkipFlagDef = enumerator2.Current;
									if (renderSkipFlagDef != RenderSkipFlagDefOf.None)
									{
										parms.skipFlags |= renderSkipFlagDef;
									}
								}
								goto IL_1A8;
							}
							goto IL_120;
						}
						goto IL_120;
						IL_1A8:
						if (apparel.def.apparel.forceEyesVisibleForRotations.Contains(parms.facing.AsInt))
						{
							parms.skipFlags &= ~RenderSkipFlagDefOf.Eyes;
							continue;
						}
						continue;
						IL_120:
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead))
						{
							parms.skipFlags |= RenderSkipFlagDefOf.Hair;
						}
						if (apparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead))
						{
							parms.skipFlags |= RenderSkipFlagDefOf.Hair;
							parms.skipFlags |= RenderSkipFlagDefOf.Beard;
							parms.skipFlags |= RenderSkipFlagDefOf.Eyes;
							goto IL_1A8;
						}
						goto IL_1A8;
					}
				}
				if (this.pawn.genes != null && !this.pawn.genes.TattoosVisible)
				{
					parms.skipFlags |= RenderSkipFlagDefOf.Tattoos;
				}
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x000AC930 File Offset: 0x000AAB30
		private bool HasValidPropsAndRenderTree
		{
			get
			{
				PawnRenderTreeDef renderTree = this.pawn.RaceProps.renderTree;
				Type left;
				if (renderTree == null)
				{
					left = null;
				}
				else
				{
					PawnRenderNodeProperties root = renderTree.root;
					left = ((root != null) ? root.nodeClass : null);
				}
				return left != null;
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000AC960 File Offset: 0x000AAB60
		private void TrySetupGraphIfNeeded()
		{
			if (this.Resolved)
			{
				return;
			}
			PawnRenderTreeDef renderTree = this.pawn.RaceProps.renderTree;
			PawnRenderNodeProperties pawnRenderNodeProperties = (renderTree != null) ? renderTree.root : null;
			if (((pawnRenderNodeProperties != null) ? pawnRenderNodeProperties.nodeClass : null) == null)
			{
				return;
			}
			this.SetDirty();
			using (new ProfilerBlock("TrySetupGraph"))
			{
				try
				{
					this.rootNode = (PawnRenderNode)Activator.CreateInstance(pawnRenderNodeProperties.nodeClass, new object[]
					{
						this.pawn,
						pawnRenderNodeProperties,
						this
					});
					this.SetupDynamicNodes();
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception setting up dynamic nodes for ");
					defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				foreach (KeyValuePair<PawnRenderNodeTagDef, List<PawnRenderNode>> keyValuePair in this.tmpChildTagNodes)
				{
					PawnRenderNodeTagDef pawnRenderNodeTagDef;
					List<PawnRenderNode> list;
					keyValuePair.Deconstruct(out pawnRenderNodeTagDef, out list);
					PawnRenderNodeTagDef key = pawnRenderNodeTagDef;
					List<PawnRenderNode> list2 = list;
					PawnRenderNode pawnRenderNode;
					if (this.nodesByTag.TryGetValue(key, out pawnRenderNode))
					{
						pawnRenderNode.AddChildren(list2.ToArray());
					}
				}
				if (Prefs.DevMode)
				{
					Dialog_DebugRenderTree dialog_DebugRenderTree = Find.WindowStack.WindowOfType<Dialog_DebugRenderTree>();
					if (dialog_DebugRenderTree != null && dialog_DebugRenderTree.pawn == this.pawn)
					{
						dialog_DebugRenderTree.Init(this.pawn);
					}
				}
				this.InitializeAncestors();
			}
			this.tmpChildTagNodes.Clear();
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000ACB38 File Offset: 0x000AAD38
		public bool TryGetNodeByTag(PawnRenderNodeTagDef tag, out PawnRenderNode node)
		{
			return this.nodesByTag.TryGetValue(tag, out node);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000ACB48 File Offset: 0x000AAD48
		private void SetupDynamicNodes()
		{
			foreach (DynamicPawnRenderNodeSetup dynamicPawnRenderNodeSetup in PawnRenderTree.dynamicNodeTypeInstances)
			{
				if (!dynamicPawnRenderNodeSetup.HumanlikeOnly || this.pawn.RaceProps.Humanlike)
				{
					foreach (ValueTuple<PawnRenderNode, PawnRenderNode> valueTuple in dynamicPawnRenderNodeSetup.GetDynamicNodes(this.pawn, this))
					{
						PawnRenderNode item = valueTuple.Item1;
						PawnRenderNode item2 = valueTuple.Item2;
						this.AddChild(item, item2);
					}
				}
			}
			foreach (ThingComp thingComp in this.pawn.AllComps)
			{
				List<PawnRenderNode> list = thingComp.CompRenderNodes();
				if (list != null)
				{
					foreach (PawnRenderNode pawnRenderNode in list)
					{
						if (this.ShouldAddNodeToTree((pawnRenderNode != null) ? pawnRenderNode.Props : null))
						{
							this.AddChild(pawnRenderNode, null);
						}
					}
				}
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000ACCA4 File Offset: 0x000AAEA4
		private void InitializeAncestors()
		{
			this.TraverseTree(delegate(PawnRenderNode node)
			{
				if (!this.nodeAncestors.ContainsKey(node))
				{
					this.nodeAncestors.Add(node, new List<PawnRenderNode>());
				}
				for (PawnRenderNode pawnRenderNode = node; pawnRenderNode != null; pawnRenderNode = pawnRenderNode.parent)
				{
					this.nodeAncestors[node].Add(pawnRenderNode);
				}
				this.nodeAncestors[node].Reverse();
			});
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000ACCB8 File Offset: 0x000AAEB8
		private void TraverseTree(Action<PawnRenderNode> action)
		{
			try
			{
				this.nodeQueue.Enqueue(this.rootNode);
				while (this.nodeQueue.Count > 0)
				{
					PawnRenderNode pawnRenderNode = this.nodeQueue.Dequeue();
					if (pawnRenderNode == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Node is null - you must called EnsureGraphicsInitialized() on the drawn dynamic thing ");
						defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
						defaultInterpolatedStringHandler.AppendLiteral(" before drawing it.");
						Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), Gen.HashCombine<int>(1743846, this.pawn.GetHashCode()));
						break;
					}
					action(pawnRenderNode);
					if (pawnRenderNode.children != null)
					{
						foreach (PawnRenderNode item in pawnRenderNode.children)
						{
							this.nodeQueue.Enqueue(item);
						}
					}
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Exception traversing pawn render node tree ");
				defaultInterpolatedStringHandler2.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			finally
			{
				this.nodeQueue.Clear();
			}
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000ACE10 File Offset: 0x000AB010
		public bool ShouldAddNodeToTree(PawnRenderNodeProperties props)
		{
			if (props == null)
			{
				return false;
			}
			switch (props.pawnType)
			{
			case PawnRenderNodeProperties.RenderNodePawnType.HumanlikeOnly:
				return this.pawn.RaceProps.Humanlike;
			case PawnRenderNodeProperties.RenderNodePawnType.NonHumanlikeOnly:
				return !this.pawn.RaceProps.Humanlike;
			}
			return true;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000ACE64 File Offset: 0x000AB064
		private void AddChild(PawnRenderNode child, PawnRenderNode parent)
		{
			if (parent == null)
			{
				PawnRenderNode pawnRenderNode;
				if (child.Props.parentTagDef != null && this.nodesByTag.TryGetValue(child.Props.parentTagDef, out pawnRenderNode))
				{
					parent = pawnRenderNode;
				}
				else
				{
					parent = this.rootNode;
				}
			}
			if (parent.Props.tagDef != null)
			{
				List<PawnRenderNode> list;
				if (this.tmpChildTagNodes.TryGetValue(parent.Props.tagDef, out list))
				{
					list.Add(child);
				}
				else
				{
					this.tmpChildTagNodes.Add(parent.Props.tagDef, new List<PawnRenderNode>
					{
						child
					});
				}
			}
			child.parent = parent;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000ACF01 File Offset: 0x000AB101
		public void SetTagNode(PawnRenderNodeTagDef tag, PawnRenderNode node)
		{
			this.nodesByTag[tag] = node;
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000ACF10 File Offset: 0x000AB110
		public void SetDirty()
		{
			this.nodeAncestors.Clear();
			this.drawRequests.Clear();
			this.rootNode = null;
			this.nodesByTag.Clear();
			this.oldParms = default(PawnDrawParms);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000ACF46 File Offset: 0x000AB146
		public bool TryGetAnimationPartForNode(PawnRenderNode node, out AnimationPart animationPart)
		{
			animationPart = null;
			return this.currentAnimation != null && node.Props.tagDef != null && node.tree.currentAnimation.TryGetPartForNodeTag(node.Props.tagDef, out animationPart);
		}

		// Token: 0x040016D6 RID: 5846
		public Pawn pawn;

		// Token: 0x040016D7 RID: 5847
		public PawnRenderNode rootNode;

		// Token: 0x040016D8 RID: 5848
		private PawnDrawParms oldParms;

		// Token: 0x040016D9 RID: 5849
		public AnimationDef currentAnimation;

		// Token: 0x040016DA RID: 5850
		public int animationStartTick = -99999;

		// Token: 0x040016DB RID: 5851
		public Color? debugTint;

		// Token: 0x040016DC RID: 5852
		private readonly List<PawnGraphicDrawRequest> drawRequests = new List<PawnGraphicDrawRequest>();

		// Token: 0x040016DD RID: 5853
		private readonly Dictionary<PawnRenderNodeTagDef, PawnRenderNode> nodesByTag = new Dictionary<PawnRenderNodeTagDef, PawnRenderNode>();

		// Token: 0x040016DE RID: 5854
		private readonly Dictionary<PawnRenderNode, List<PawnRenderNode>> nodeAncestors = new Dictionary<PawnRenderNode, List<PawnRenderNode>>();

		// Token: 0x040016DF RID: 5855
		private readonly Dictionary<PawnRenderNodeTagDef, List<PawnRenderNode>> tmpChildTagNodes = new Dictionary<PawnRenderNodeTagDef, List<PawnRenderNode>>();

		// Token: 0x040016E0 RID: 5856
		private static readonly List<DynamicPawnRenderNodeSetup> dynamicNodeTypeInstances = new List<DynamicPawnRenderNodeSetup>();

		// Token: 0x040016E1 RID: 5857
		private const float ApparelLayerShellNorth = 88f;

		// Token: 0x040016E2 RID: 5858
		private readonly Queue<PawnRenderNode> nodeQueue = new Queue<PawnRenderNode>();
	}
}
