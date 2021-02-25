namespace ET
{
    public static class SceneHelper
    {
        public static int DomainZone(this Entity entity)
        {
            return ((Scene) entity.Domain).Zone;
        }

        public static Scene DomainScene(this Entity entity)
        {
            return (Scene) entity.Domain;
        }

        public static long RandomMapsInstanceId(int zone)
        {
            return StartSceneConfigCategory.Instance.GetBySceneName(zone, "Map").SceneId;
        }
    }
}