using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001842 RID: 6210
	public class CompCableConnection : ThingComp
	{
		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x06009B76 RID: 39798 RVA: 0x0038085D File Offset: 0x0037EA5D
		public CompProperties_CableConnection Props
		{
			get
			{
				return (CompProperties_CableConnection)this.props;
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x06009B77 RID: 39799 RVA: 0x0038086A File Offset: 0x0037EA6A
		public Building ParentBuilding
		{
			get
			{
				return this.parent as Building;
			}
		}

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x06009B78 RID: 39800 RVA: 0x00380877 File Offset: 0x0037EA77
		public bool CanMote
		{
			get
			{
				return this.ParentBuilding.IsWorking();
			}
		}

		// Token: 0x06009B79 RID: 39801 RVA: 0x00380884 File Offset: 0x0037EA84
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Props.drawMote || !this.CanMote)
			{
				return;
			}
			foreach (CompCableConnection.Cable cable in this.cables)
			{
				cable.Tick();
			}
		}

		// Token: 0x06009B7A RID: 39802 RVA: 0x003808F0 File Offset: 0x0037EAF0
		public override void PostDraw()
		{
			base.PostDraw();
			foreach (CompCableConnection.Cable cable in this.cables)
			{
				cable.Draw();
			}
		}

		// Token: 0x06009B7B RID: 39803 RVA: 0x00380948 File Offset: 0x0037EB48
		private Vector3 GetTargetConnectionPt(Thing target, ref int idx)
		{
			ThingWithComps thingWithComps = target as ThingWithComps;
			int num;
			if (thingWithComps != null)
			{
				CompAttachPoints comp = thingWithComps.GetComp<CompAttachPoints>();
				if (comp != null)
				{
					AttachPointTracker points = comp.points;
					AttachPointType[] cableAttachPoints = this.CableAttachPoints;
					num = idx;
					idx = num + 1;
					return points.GetWorldPos(cableAttachPoints[num % this.CableAttachPoints.Length]);
				}
			}
			Vector3 drawPos = target.DrawPos;
			List<Vector3> list = this.Props.offsets[this.parent.Rotation.AsInt];
			num = idx;
			idx = num + 1;
			return drawPos + list[num];
		}

		// Token: 0x06009B7C RID: 39804 RVA: 0x003809CC File Offset: 0x0037EBCC
		public void RebuildCables(List<Thing> connections, Func<Thing, bool> connectionValidator = null)
		{
			this.cables.Clear();
			int num = 0;
			foreach (Thing thing in connections)
			{
				if (connectionValidator == null || connectionValidator(thing))
				{
					Vector3 worldPos = this.parent.GetComp<CompAttachPoints>().points.GetWorldPos(AttachPointType.CableConnection0);
					Vector3 targetConnectionPt = this.GetTargetConnectionPt(thing, ref num);
					Vector3 vector = worldPos - targetConnectionPt;
					Vector2 vector2 = new Vector2(vector.x, vector.z);
					Vector2 a = vector2 * 0.5f;
					Vector3 pos = targetConnectionPt;
					pos.y += -4.9268293f;
					Vector2 vector3 = Vector2.Perpendicular(vector2 * 0.4f);
					bool flag = Vector2.Dot(vector3.normalized, Vector2.up) < 0f;
					if (flag)
					{
						vector3 = -vector3;
					}
					float degrees = flag ? CompCableConnection.CableRotRange1 : CompCableConnection.CableRotRange2;
					float degrees2 = flag ? CompCableConnection.CableRotRange2 : CompCableConnection.CableRotRange1;
					Vector2[] array = LineMeshGenerator.CalculateEvenlySpacedPoints(new List<Vector2>
					{
						new Vector2(0f, 0f),
						a + vector3.RotatedBy(degrees),
						a + vector3.RotatedBy(degrees2),
						vector2
					}, 0.2f, 1f);
					Mesh mesh = LineMeshGenerator.Generate(array, 0.15f);
					Material mat = MaterialPool.MatFrom(new MaterialRequest
					{
						mainTex = ContentFinder<Texture2D>.Get("Things/Building/Cable", true),
						shader = ShaderDatabase.Transparent,
						color = this.Props.color
					});
					List<ValueTuple<Vector2, float>> list = new List<ValueTuple<Vector2, float>>();
					int i = 1;
					int num2 = array.Length;
					while (i < num2)
					{
						Vector2 vector4 = array[i];
						Vector2 b = array[i - 1];
						float item = vector4.AngleTo(b) + 90f;
						list.Add(new ValueTuple<Vector2, float>(vector4, item));
						i++;
					}
					CompCableConnection.Cable item2 = new CompCableConnection.Cable
					{
						mesh = mesh,
						mat = mat,
						pos = pos,
						map = this.parent.Map,
						points = list,
						quat = Quaternion.identity,
						moteDef = this.Props.moteDef
					};
					this.cables.Add(item2);
				}
			}
		}

		// Token: 0x04005C65 RID: 23653
		private readonly List<CompCableConnection.Cable> cables = new List<CompCableConnection.Cable>();

		// Token: 0x04005C66 RID: 23654
		private const string CableTexturePath = "Things/Building/Cable";

		// Token: 0x04005C67 RID: 23655
		private const float CableYOffset = -5f;

		// Token: 0x04005C68 RID: 23656
		private const float CableLineMeshPointsSpacing = 0.2f;

		// Token: 0x04005C69 RID: 23657
		private const float CableLineMeshWidth = 0.15f;

		// Token: 0x04005C6A RID: 23658
		private static readonly float CableRotRange1 = -Rand.Range(35f, 40f);

		// Token: 0x04005C6B RID: 23659
		private static readonly float CableRotRange2 = Rand.Range(20f, 25f);

		// Token: 0x04005C6C RID: 23660
		private readonly AttachPointType[] CableAttachPoints = new AttachPointType[]
		{
			AttachPointType.CableConnection0,
			AttachPointType.CableConnection1,
			AttachPointType.CableConnection2,
			AttachPointType.CableConnection3
		};

		// Token: 0x02003612 RID: 13842
		private class Cable
		{
			// Token: 0x06013C4A RID: 80970 RVA: 0x0066D24B File Offset: 0x0066B44B
			public void Draw()
			{
				Graphics.DrawMesh(this.mesh, this.pos, this.quat, this.mat, 0);
			}

			// Token: 0x06013C4B RID: 80971 RVA: 0x0066D26C File Offset: 0x0066B46C
			public void Tick()
			{
				if (this.motePower == null || this.motePower.Destroyed)
				{
					ValueTuple<Vector2, float> valueTuple = this.points.RandomElement<ValueTuple<Vector2, float>>();
					Vector3 b = valueTuple.Item1.ToVector3();
					float exactRot = valueTuple.Item2 + 180f;
					this.motePower = MoteMaker.MakeStaticMote(this.pos + b, this.map, this.moteDef, 1f, false, exactRot);
				}
			}

			// Token: 0x0400D814 RID: 55316
			public Mesh mesh;

			// Token: 0x0400D815 RID: 55317
			public Vector3 pos;

			// Token: 0x0400D816 RID: 55318
			public Quaternion quat;

			// Token: 0x0400D817 RID: 55319
			public Material mat;

			// Token: 0x0400D818 RID: 55320
			[TupleElementNames(new string[]
			{
				"offset",
				"rot"
			})]
			public List<ValueTuple<Vector2, float>> points;

			// Token: 0x0400D819 RID: 55321
			public Map map;

			// Token: 0x0400D81A RID: 55322
			public ThingDef moteDef;

			// Token: 0x0400D81B RID: 55323
			private Mote motePower;
		}
	}
}
