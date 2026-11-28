using System;
using LudeonTK;
using UnityEngine;
using Verse;

// Token: 0x02000005 RID: 5
public class DevWindowDrawing : IWindowDrawing
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000014 RID: 20 RVA: 0x0000240E File Offset: 0x0000060E
	public GUIStyle EmptyStyle
	{
		get
		{
			return DevGUI.EmptyStyle;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002418 File Offset: 0x00000618
	public bool DoCloseButton(Rect rect, string text)
	{
		return DevGUI.ButtonText(rect, text, null);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002438 File Offset: 0x00000638
	public bool DoClostButtonSmall(Rect rect)
	{
		return DevGUI.ButtonImage(new Rect(rect.x + rect.width - 22f - 4f, rect.y + 4f, 22f, 22f), DevGUI.Close);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002486 File Offset: 0x00000686
	public void DoGrayOut(Rect rect)
	{
		DevGUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000024A7 File Offset: 0x000006A7
	public void DoWindowBackground(Rect rect)
	{
		GUI.color = DevGUI.WindowBGFillColor;
		GUI.DrawTexture(rect, BaseContent.WhiteTex);
		GUI.color = Color.white;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000024C8 File Offset: 0x000006C8
	public void BeginGroup(Rect rect)
	{
		DevGUI.BeginGroup(rect);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000024D0 File Offset: 0x000006D0
	public void EndGroup()
	{
		DevGUI.EndGroup();
	}
}
