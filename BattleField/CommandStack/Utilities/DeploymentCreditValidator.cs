using System.Collections.Generic;
using CommandStack.Commands;
using Models.Constants;
using Models.Enums;

namespace CommandStack.Utilities
{
    internal static class DeploymentCreditValidator
    {
        public static bool ValidDeployment(IEnumerable<Deploy> deployments)
        {
            int cost = 0;
            foreach (var deploy in deployments)
            {
                switch (deploy.Type)
                {
                    case UnitType.TankHeavy:
                    case UnitType.TankLight:
                        cost += 3;
                        break;
                    case UnitType.Infantry:
                        cost += 1;
                        break;
                    case UnitType.PlaneBomber:
                    case UnitType.PlaneFighter:
                        cost += 2;
                        break;
                }
            }
            return cost <= MoveInfo.DEPLOYMENT_CREDITS;
        }
    }
}
