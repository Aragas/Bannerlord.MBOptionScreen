namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
        enum UnavailableSettingType
    {
        PerCampaign,
        PerSave
    }
}