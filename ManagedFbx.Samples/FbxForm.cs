﻿using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagedFbx;

public partial class FbxForm : Form
{
	public FbxForm()
	{
		InitializeComponent();
		SetTitle("Untitled");
	}

	public void Add(SceneNode node, TreeNode parentNode)
	{
		var item = new TreeNode(node.Name) { Tag = node };

		if(parentNode == null)
			uxFbxTree.Nodes.Add(item);
		else
			parentNode.Nodes.Add(item);

		foreach(var sub in node.ChildNodes)
		{
			var subitem = new TreeNode(sub.Name) { Tag = sub };
			Add(sub, item);
		}
	}

	private void OnTreeSelect(object sender, TreeViewEventArgs e)
	{
		var node = e.Node.Tag as SceneNode;

		if(node == null)
			return;

        var builder = new StringBuilder(40000);

		Action NewLine = () => builder.Append(Environment.NewLine);

		builder.Append("Position:\t{0}", node.Position);
		builder.Append("Rotation:\t{0}", node.Rotation);
		builder.Append("Scale:\t{0}", node.Scale);

		NewLine();

		builder.Append("Found {0} attribute(s)", node.Attributes.Count());

		foreach(var attr in node.Attributes)
		{
			NewLine();
			builder.Append("Attribute type: {0}", attr.Type.ToString());

			switch(attr.Type)
			{
				case NodeAttributeType.Mesh:
					{
						var mesh = node.Mesh;

						if(!mesh.Triangulated)
						{
							builder.Append("Quads/ngons found in list of total {0} polygons, triangulating", mesh.Polygons.Length);
							NewLine();
							mesh = mesh.Triangulate();
						}

						builder.Append("Found {0} triangles", mesh.Polygons.Length);
						NewLine();

						for(var i = 0; i < mesh.Polygons.Length; i++)
						{
							var str = string.Empty;
							foreach(var index in mesh.Polygons[i].Indices)
								str += "\t" + index;

							builder.Append("{0}:{1}\t(UVs: {2}, {3}, {4}, Mat ID: {5})", i, str, mesh.GetUVIndex(i, 0), mesh.GetUVIndex(i, 1), mesh.GetUVIndex(i, 2), mesh.GetMaterialId(i));
						}

						NewLine();
						builder.Append("Found {0} vertices", mesh.Vertices.Length);
						NewLine();

						for(var i = 0; i < mesh.Vertices.Length; i++)
						{
							var vertex = mesh.Vertices[i];
							builder.Append("{0}:\t{1}\t{2}\t{3}", i, Math.Round(vertex.X, 2), Math.Round(vertex.Y, 2), Math.Round(vertex.Z, 2));
						}

						NewLine();
						builder.Append("Found {0} vertex normals", mesh.Normals.Length);
						NewLine();

						for(var i = 0; i < mesh.Normals.Length; i++)
						{
							var normal = mesh.Normals[i];
							builder.Append("{0}:\t{1}\t{2}\t{3}", i, Math.Round(normal.X, 2), Math.Round(normal.Y, 2), Math.Round(normal.Z, 2));
						}

						NewLine();
						builder.Append("Found {0} UV coords", mesh.TextureCoords.Length);
						NewLine();

						for(var i = 0; i < mesh.TextureCoords.Length; i++)
						{
							var coord = mesh.TextureCoords[i];
							builder.Append("{0}:\t{1}\t{2}", i, Math.Round(coord.X, 2), Math.Round(coord.Y, 2));
						}

						NewLine();
						builder.Append("Found {0} vertex colours", mesh.VertexColours.Length);
						NewLine();

						for(var i = 0; i < mesh.VertexColours.Length; i++)
						{
							var colour = mesh.VertexColours[i];
							builder.Append("{0}:\t{1}\t{2}\t{3}\t{4}", i, Math.Round(colour.R, 2), Math.Round(colour.G, 2), Math.Round(colour.B, 2), Math.Round(colour.A, 2));
						}

						NewLine();
						builder.Append("Found {0} material IDs", mesh.MaterialIDs.Length);
						NewLine();

						for(var i = 0; i < mesh.MaterialIDs.Length; i++)
						{
							var id = mesh.MaterialIDs[i];
							builder.Append("{0}:\t{1}", i.ToString(), id.ToString());
						}
					}
					break;

				case NodeAttributeType.Light:
					{
						var light = node.Light;
						builder.Append("Found light of type {0}", light.Type);
						builder.Append("Colour is {0}", light.Colour);
					}
					break;
			}
		}

		uxNodeInfo.Text = builder.ToString();
	}

	private void LoadFile(object sender, EventArgs e)
	{
		var dialog = new OpenFileDialog();
		dialog.Filter = "FBX files (*.fbx)|*.fbx";

		if(dialog.ShowDialog() == DialogResult.OK)
		{
			var scenePath = dialog.FileName;
			m_scene = Scene.Import(scenePath);
			uxFbxTree.Nodes.Clear();
			Add(m_scene.RootNode, null);
			SetTitle(scenePath);
		}
	}

	private void SaveFile(object sender, EventArgs e)
	{
		var dialog = new SaveFileDialog();
		dialog.Filter = "FBX file (*.fbx)|*.fbx";

		if(dialog.ShowDialog() == DialogResult.OK)
		{
			var filePath = dialog.FileName;
			m_scene.Save(filePath);
		}
	}

	private void SetTitle(string filename)
	{
		Text = "FBX Viewer - " + filename;
	}

	private Scene m_scene;
}

public static class StringBuilderExtensions
{
	public static void Append(this StringBuilder builder, string format, params object[] args)
	{
		builder.AppendLine(string.Format(format, args));
	}
}