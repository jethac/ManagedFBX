#pragma once

namespace ManagedFbx
{
	public enum class NodeAttributeType
	{
		Unknown = FbxNodeAttribute::eUnknown,
		Null = FbxNodeAttribute::eNull,
		Marker = FbxNodeAttribute::eMarker,
		Skeleton = FbxNodeAttribute::eSkeleton,
		Mesh = FbxNodeAttribute::eMesh,
		Nurbs = FbxNodeAttribute::eNurbs,
		Patch = FbxNodeAttribute::ePatch,
		Camera = FbxNodeAttribute::eCamera,
		CameraStereo = FbxNodeAttribute::eCameraStereo,
		CameraSwitcher = FbxNodeAttribute::eCameraSwitcher,
		Light = FbxNodeAttribute::eLight,
		OpticalReference = FbxNodeAttribute::eOpticalReference,
		OpticalMarker = FbxNodeAttribute::eOpticalMarker,
		NurbsCurve = FbxNodeAttribute::eNurbsCurve,
		TrimNurbsSurface = FbxNodeAttribute::eTrimNurbsSurface,
		Boundary = FbxNodeAttribute::eBoundary,
		NurbsSurface = FbxNodeAttribute::eNurbsSurface,
		Shape = FbxNodeAttribute::eShape,
		LodGroup = FbxNodeAttribute::eLODGroup,
		Subdiv = FbxNodeAttribute::eSubDiv
	};

	public ref class NodeAttribute
	{
	public:
		property_r(NodeAttributeType, Type);
		property_rw(string^, Name);

	internal:
		NodeAttribute(FbxNodeAttribute *nativeAttr);
		FbxNodeAttribute *m_nativeAttribute;
	};
}