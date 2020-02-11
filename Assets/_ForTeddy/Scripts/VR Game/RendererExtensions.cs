using UnityEngine;

public static class RendererExtensions
{
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        var visibilityLimits = renderer.bounds;
        visibilityLimits.size /= 2;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, visibilityLimits);
    }
}