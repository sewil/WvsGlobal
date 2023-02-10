using System;
using System.Diagnostics;
using System.Linq;
using WvsBeta.Common.Enums;
using WvsBeta.Game;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Scripts.Scripts
{
    public static class face
    {
        static INpcHost self;
        static GameCharacter target;
        public static void SayFaceResult(int mFace)
        {
            if (mFace == 1) self.Say("Okay, the procedure is over. Look, here's a mirror. What do you think? Even I admit this is a work of art... haha, well, call me when you get sick of this new look, okay?");
            else if (mFace == -1) self.Say("Um... Looks like you don't have the specific coupon for this place... Sorry to say this, but without the coupon, no plastic surgery for you.");
            else if (mFace == -3) self.Say("Um... It looks like we have a problem here at the hospital, and I feel like I can't proceed with the procedure right away. Please come back later.");
            else if (mFace == 0 || mFace == -2) self.Say("Hmm... There seems to be a problem with the procedure here. Please come back later.");
        }
        public static void AskVIPFace(string text, int couponid)
        {
            var mFace = self.AskAvatar(text, couponid, GetFaces());
            SayFaceResult(mFace);
        }
        public static void AskRegularFace(int couponid)
        {
            var nRet = self.AskYesNo("If you use the regular coupon, your face can change into anyone... Still want to do that using #b#t" + couponid + "##k?");
            if (nRet == 0)
            {
                self.Say("I understand... Take a moment to think and decide if this is really what you want. When you have made up your mind, let me know.");
                return;
            }
            var mFace = self.MakeRandAvatar(couponid, GetFaces());
            SayFaceResult(mFace);
        }
        public static int[] GetFaces()
        {
            int teye = (target.Gender * 1000) + (target.Face / 100 % 10 * 100);
            int[] faces = new int[10];
            for (int i = 0; i < faces.Length; i++)
            {
                faces[i] = 20000 + i + teye;
            }
            return faces;
        }
        // Henesys VIP plastic surgery
        [Script("face_henesys1")]
        class face_henesys1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskVIPFace("Let's see... I can turn your face into something totally new. Do you want to try? With #b#t4052001##k, you can get a face of your choice. Calmly select a face that you prefer...", 4052001);
            }
        }
        // Henesys regular plastic surgery
        [Script("face_henesys2")]
        class face_henesys2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskRegularFace(4052000);
            }
        }
        // Orbis VIP plastic surgery
        [Script("face_orbis1")]
        class face_orbis1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskVIPFace("Well, well, well, welcome!! I can transform your face into a whole new thing... How about giving us a chance? With #b#t4052005##k, you can have a face that you like... Take your time to choose one that you like...", 4052005);
            }
        }
        // Orbis regular plastic surgery
        [Script("face_orbis2")]
        class face_orbis2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskRegularFace(4052004);
            }
        }
        // Ludibrium VIP plastic surgery
        [Script("face_ludi1")]
        class face_ludi1 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskVIPFace("Let's see... I can turn your face into something totally new. Do you want to try? With #b#t4052007##k, you can get a face of your choice. Calmly select a face that you prefer...", 4052007);
            }
        }
        // Ludibrium regular plastic surgery
        [Script("face_ludi2")]
        class face_ludi2 : INpcScript
        {
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                AskRegularFace(4052006);
            }
        }
        [Script("lens_henesys1")]
        class lens_henesys1 : INpcScript
        {
            dynamic changeEye4, changeEye3, changeEye2, mFace, changeEye1, nRet1, nRet, tface, changeEye5, changeEye6;
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                nRet = self.AskMenu("Hi there! I'm Dr. Lenu, responsible for cosmetic lenses here at the Henesys Plastic Surgery Store! With #b#t5152010##k or #b#t5152013##k, you can let us take care of the rest and get the gorgeous look you've always wanted! Remember, the first thing everyone notices about you is your eyes, and we'll help you find the contact lenses that look best on you! Now, what would you like to use?\r\n#b#L0# Henesys Contact Lens Coupon#l\r\n#L1# Henesys VIP Contact Lens Coupon#l");
                
                tface = target.Face - (target.Face / 100) % 10 * 100;
                changeEye1 = tface;
                changeEye2 = tface + 100;
                changeEye3 = tface + 400;
                changeEye4 = tface + 500;
                changeEye5 = tface + 600;
                changeEye6 = tface + 700;

                if (nRet == 0)
                {
                    nRet1 = self.AskYesNo("If you use the regular coupon, you will be rewarded with a pair of lenses. Are you going to use #b#t5152010##k and really make a difference to your eyes?");
                    if (nRet1 == 0)
                    {
                        self.Say("I understand. This is understandable, as long as you're not sure you want to have the contact lenses you like. Anyway, we're not in the rush, so think about it! Let me know when you decide to make the switch!");
                        return;
                    }
                    else if (nRet1 == 1)
                    {
                        mFace = self.MakeRandAvatar(5152010, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);
                    }
                }
                else if (nRet == 1)
                {
                    mFace = self.AskAvatar("With our special machine, you can see in advance how it will look after the treatment. What kind of lenses would you like to use? Choose the style according to your taste...", 5152013, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);
                }

                if (mFace == 1) self.Say("Ta-da! Oh, check out the look!! What do you think? I really think your eyes look fantastic now! Please come back again.");
                else if (mFace == -1) self.Say("Sorry, but I think now you don't have our contact lenses coupon. Without the coupon, I'm sorry but I can't make it for you.");
                else if (mFace == -3) self.Say("I'm sorry, but it looks like our contact lens making machine is not working right now. Please come back later. I'm really sorry!");
                else if (mFace == 0 || mFace == -2) self.Say("Hmm... There seems to be a problem with the procedure here. Please check back with me later.");
            }
        }
        [Script("lens_orbis1")]
        class lens_orbis1 : INpcScript
        {
            dynamic changeEye4, changeEye3, changeEye2, mFace, changeEye1, nRet1, nRet, tface, changeEye5, changeEye6;
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                nRet = self.AskMenu("Olá, sou o Dr. Rhome, o cabeça do departamento de lentes de contato aqui na Loja de Cirurgia Plástica de Orbis. Minha meta aqui é dar personalidade para os olhos de todos por meio das maravilhas das lentes cosméticas e, com #b#t5152011##k ou #b#t5150104##k, também posso fazer o mesmo por você! Agora, o que você gostaria de usar?\r\n#b#L0# Cupom de Lentes de Contato em Orbis#l\r\n#L1# Cupom de Lentes de Contato VIP em Orbis#l");
                if (nRet == 0)
                {
                    nRet1 = self.AskYesNo("Saiba que, usando o cupom normal, ganhará um par aleatório de lentes de contato. Vai usar #b#t5152011##k e realmente fazer a diferença dos seus olhos?");
                    if (nRet1 == 0) self.Say("Entendo sua dúvida. Tem bastante tempo para pensar sobre o assunto. Fale comigo quando decidir fazer a mudança!");
                    else if (nRet1 == 1)
                    {
                        tface = target.Face - (target.Face / 100) % 10 * 100;
                        changeEye1 = tface;
                        changeEye2 = tface + 100;
                        changeEye3 = tface + 200;
                        changeEye4 = tface + 400;
                        changeEye5 = tface + 600;
                        changeEye6 = tface + 700;

                        mFace = self.MakeRandAvatar(5152011, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);

                        if (mFace == 1) self.Say("Certo, olhe-se no espelho! O que você acha? As lentes caíram como uma luva em você! Parecem tão naturais em você! Espero vê-lo novamente!");
                        else if (mFace == -1) self.Say("Desculpe, mas acho que agora você não tem o cupom de nossas lentes de contato. Sem o cupom, sinto muito, mas não posso fazer isso para você.");
                        else if (mFace == -3) self.Say("Desculpe, mas parece que nossa máquina de fazer lentes de contato não está funcionando agora. Por favor, volte depois. Me desculpe, de verdade!");
                        else if (mFace == 0 || mFace == -2) self.Say("Hum... Parece haver um problema com o procedimento aqui. Por favor, confirme comigo mais tarde.");
                    }
                }
                else if (nRet == 1)
                {
                    tface = target.Face - (target.Face / 100) % 10 * 100;
                    changeEye1 = tface;
                    changeEye2 = tface + 100;
                    changeEye3 = tface + 200;
                    changeEye4 = tface + 400;
                    changeEye5 = tface + 600;
                    changeEye6 = tface + 700;

                    mFace = self.AskAvatar("Com nossa máquina especial, você pode ver antes como ficará depois do tratamento. Que tipo de lentes gostaria de usar? Escolha o estilo de acordo com seu gosto...", 5152014, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);

                    if (mFace == 1) self.Say("Certo, olhe-se no espelho! O que você acha? As lentes caíram como uma luva em você! Parecem tão naturais em você! Espero vê-lo novamente!");
                    else if (mFace == -1) self.Say("Desculpe, mas acho que agora você não tem o cupom de nossas lentes de contato. Sem o cupom, sinto muito, mas não posso fazer isso para você.");
                    else if (mFace == -3) self.Say("Desculpe, mas parece que nossa máquina de fazer lentes de contato não está funcionando agora. Por favor, volte depois. Me desculpe, de verdade!");
                    else if (mFace == 0 || mFace == -2) self.Say("Desculpe, mas parece haver um problema com o procedimento aqui. Por favor, confirme comigo mais tarde.");
                }
            }
        }
        [Script("lens_ludi1")]
        class lens_ludi1 : INpcScript
        {
            dynamic changeEye4, changeEye3, changeEye2, mFace, changeEye1, nRet1, nRet, tface, changeEye5, changeEye6;
            public void Run(INpcHost self, GameCharacter target)
            {
                face.self = self;
                face.target = target;
                nRet = self.AskMenu("Hum... Oi, sou o Dr. Bosch e sou o especialista em lentes aqui na Loja de Cirurgia Plástica de Ludibrium. Acredito que seus olhos são as coisas mais importantes em seu corpo e, com #b#t5152012##k ou #b#t5152015##k, posso prescrerver as lentes certas para você. Agora, o que você gostaria de usar?\r\n#b#L0# Cupom de Lentes de Contato em Ludibrium#l\r\n#L1# Cupom de Lentes de Contato VIP em Ludibrium#l");
                if (nRet == 0)
                {
                    nRet1 = self.AskYesNo("Tenho que avisar que, se usar o cupom comum, você será recompensado com um par de lentes. Vai usar #b#t5152012##k e realmente fazer a diferença dos seus olhos?");
                    if (nRet1 == 0) self.Say("É compreensível, desde que haja uma chance de você não receber o tipo de lentes que estava procurando. Se ainda as quiser, então fale comigo.");
                    else if (nRet1 == 1)
                    {
                        tface = target.Face - (target.Face / 100) % 10 * 100;
                        changeEye1 = tface;
                        changeEye2 = tface + 100;
                        changeEye3 = tface + 300;
                        changeEye4 = tface + 400;
                        changeEye5 = tface + 600;
                        changeEye6 = tface + 700;

                        mFace = self.MakeRandAvatar(5152012, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);

                        if (mFace == 1) self.Say("Aqui está o espelho. O que você acha? Acho que estão sob medida para você. Tenho que dizer: está fabuloso. Por favor, volte novamente.");
                        else if (mFace == -1) self.Say("Desculpe, mas acho que agora você não tem o cupom de nossas lentes de contato. Sem o cupom, sinto muito, mas não posso fazer isso para você. Desculpe.");
                        else if (mFace == -3) self.Say("Desculpe, mas parece que nossa máquina de fazer lentes de contato não está funcionando agora. Por favor, volte depois. Me desculpe, de verdade!");
                        else if (mFace == 0 || mFace == -2) self.Say("Desculpe, mas parece haver um problema com o procedimento aqui. Por favor, confirme comigo mais tarde.");
                    }
                }
                else if (nRet == 1)
                {
                    tface = target.Face - (target.Face / 100) % 10 * 100;
                    changeEye1 = tface;
                    changeEye2 = tface + 100;
                    changeEye3 = tface + 300;
                    changeEye4 = tface + 400;
                    changeEye5 = tface + 600;
                    changeEye6 = tface + 700;

                    mFace = self.AskAvatar("Com nossa máquina especial, você pode ver antes como ficará depois do tratamento. Que tipo de lentes gostaria de usar? Escolha o estilo de acordo com seu gosto...", 5152015, changeEye1, changeEye2, changeEye3, changeEye4, changeEye5, changeEye6);

                    if (mFace == 1) self.Say("Aqui está o espelho. O que você acha? Acho que estão sob medida para você. Tenho que dizer: está fabuloso. Por favor, volte novamente.");
                    else if (mFace == -1) self.Say("Desculpe, mas acho que agora você não tem o cupom de nossas lentes de contato. Sem o cupom, sinto muito, mas não posso fazer isso para você. Desculpe.");
                    else if (mFace == -3) self.Say("Desculpe, mas parece que nossa máquina de fazer lentes de contato não está funcionando agora. Por favor, volte depois. Me desculpe, de verdade!");
                    else if (mFace == 0 || mFace == -2) self.Say("Desculpe, mas parece haver um problema com o procedimento aqui. Por favor, confirme comigo mais tarde.");
                }
            }
        }
    }
}
