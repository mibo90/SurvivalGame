﻿using UnityEngine;
using Svelto.ECS.Example.Survive.Sound;

namespace Svelto.ECS.Example.Survive.Bonus
{
    [DisallowMultipleComponent]
    public class BonusHealthEntityDescriptorHolder : GenericEntityDescriptorHolder<GenericEntityDescriptor<BonusHealthEntityView,BonusSoundEntityView>>
    { }
}
