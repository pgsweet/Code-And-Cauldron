

public static class ContainerHelper
{
    public static bool isContainerEmpty(ContainerScript[] containers, int containerNum)
    {
        return containers[containerNum].getItemCount() <= 0;
    }
}