namespace Air {

    public static class GameInputDomain {

        public static void Player_BakeInput(GameBusinessContext ctx, float dt) {
            InputEntity inputEntity = ctx.inputEntity;
            inputEntity.ProcessInput(ctx.mainCamera, dt);
        }

        public static void Owner_BakeInput(GameBusinessContext ctx, BoidEntity owner) {
            InputEntity inputEntity = ctx.inputEntity;
            ref BoidInputComponent inputCom = ref owner.inputCom;
            inputCom.moveAxis = inputEntity.moveAxis;
        }

    }

}