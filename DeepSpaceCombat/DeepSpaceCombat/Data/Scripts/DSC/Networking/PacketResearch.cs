using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Game;
using VRage.Game;



namespace DSC
{
    // An example packet extending another packet.
    // Note that it must be ProtoIncluded in PacketBase for it to work.
    [ProtoContract]
    public class PacketResearch : PacketBase
    {
        public PacketResearch() { } // Empty constructor required for deserialization

        // tag numbers in this class won't collide with tag numbers from the base class
        [ProtoMember(1)]
        public string Type;

        [ProtoMember(2)]
        public long PlayerId;

        [ProtoMember(3)]
        public string TypeId;

        [ProtoMember(4)]
        public string SubTypeId;

        public PacketResearch(string type, long playerId, string typeId, string subTypeId)
        {
            Type = type;
            PlayerId = playerId;
            TypeId = typeId;
            SubTypeId = subTypeId;
        }

        public override bool Received()
        {
            if (DeepSpaceCombat.Instance._isClientRegistered)
            {

                // Init research -> Set all definitions public false
                if (Type.Equals("Init"))
                {
                    //MyVisualScriptLogicProvider.ClearAllToolbarSlots(PlayerId);

                    foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                    {
                        var cubeDef = def as MyCubeBlockDefinition;
                        if (cubeDef != null)
                        {
                            // Ignore Assemblers TODO Check if we have to activate all Assembler types
                            if (cubeDef.Id.ToString().Contains("BasicAssembler"))
                            {
                                continue;
                            }
                            cubeDef.Public = false;
                        }
                    }
                }


                if (Type.Equals("Research"))
                {
                    if (SubTypeId.Contains("null"))
                    {
                        SubTypeId = null;
                    }

                    MyDefinitionId defId =  MyVisualScriptLogicProvider.GetDefinitionId(TypeId, SubTypeId);

                    if(defId != null)
                    {
                        MyDefinitionBase def = MyDefinitionManager.Static.GetDefinition(defId);

                        def.Public = true;
                    }
                    else
                    {
                        DeepSpaceCombat.Instance.ClientLogger.WriteInfo("Could not find definitionId");
                    }
                }

                if (Type.Equals("Freebuild"))
                {
                    foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
                    {
                        var cubeDef = def as MyCubeBlockDefinition;
                        if (cubeDef != null)
                        {
                            cubeDef.Public = true;
                        }
                    }
                }

            }

            return false;
        }
    }
}