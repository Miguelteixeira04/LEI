import random as rd
rd.seed()

jogador_atual = " "
matriz = [[" "," "," "," "], [" "," "," "," "], [" "," "," "," "]]

def menu():

    print("\n┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓                                ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓")                                        
    print("┃\tBem-vindo ao jogo matemático do semáforo    ┃                                ┃Trabalho realizado por:          ┃")
    print("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛                                ┃   Diogo Cabral         al78834  ┃")
    print("Jogar uma partida (1)                                                                ┃   Maria Inês Cardoso   al78222  ┃")
    print("Carregar uma partida apartir de um ficheiro (2)                                      ┃   Miguel Teixeira      al78321  ┃")
    print("Apresentar uma descrição do jogo / Regras (3)                                        ┃                                 ┃")
    print("Sair da aplicação (4)                                                                ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛\n")


    menu = int(input(""))
    
    if menu == 1:
        jogar()
    if menu == 2:
        guardado()
    if menu == 3:
        regras()
    if menu == 4:
        return
    
def jogar():
    print("\nJogar 1v1 (1)")
    print("Jogar contra um bot (2)\n")    
    
    jogar1 = int(input(""))
    
    if jogar1 == 1:
        nome1 = input("\nInsira o nome do jogador 1: ")
        nome2 = input("Insira o nome do jogador 2: ")
        
        primeiro = rd.randint(1,2)
                
        if primeiro == 1:
            print("\nO primeiro jogador é o " + nome1)
            print("O segundo jogador é o " + nome2)
            primeiro = nome1
            segundo = nome2
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2, dificuldade)
        else:
            print("\nO primeiro jogador é o " + nome2)
            print("O segundo jogador é o " + nome1)
            primeiro = nome2
            segundo = nome1
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2,dificuldade)
     
        
    elif jogar1 == 2:
        nome1 = input("\nInsira o nome do jogador: ")
           
        primeiro = rd.randint(1,2)
        
        if primeiro == 1:
            nome2 = "BOT"
            print("\nO primeiro jogador é o " + nome1)
            print(f"O segundo jogador é o {nome2}\n")
            print("\n")
            primeiro = nome1
            segundo = nome2
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2,dificuldade)
        else:
            nome2 = "BOT"
            print(f"\nO primeiro jogador é o {nome2}")
            print(f"O segundo jogador é o {nome1}")    
            print("\n")   
            primeiro = nome2
            segundo = nome1
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2,dificuldade) 
    else: 
        print("Escolha o numero 1 ou 2.\n")
        jogar()

def guardado():
    print("Ola")
    voltar()
    
def voltar() : 
    opcao = input("Pressione '2' para voltar atrás ou '3' para voltar ao menu :  ")
    print("\n")
    if opcao == '2':
     regras()
    if opcao == '3':
     menu()
    else:
        print("Insira um número válido!")

def regras():
    escolha = int(input("Apresentar uma descrição do jogo (1) \t Regras do jogo (2) :  "))
    if escolha == 1 :
        print("\n")
        print("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓")
        print("┃            Descrição do jogo           ┃")
        print("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛\n")
        print("* Autor: Alan Parr \n")
        print("* Material: 8 peças verdes, 8 amarelas e 8 vermelhas partilhadas pelos jogadores.\n")
        print("* Objetivo: Ser o primeiro a conseguir uma linha de três peças da mesma cor na horizontal, vertical ou diagonal.\n")
        voltar()
    else:
        print("\n")
        print("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓")
        print("┃              Regras do jogo            ┃")
        print("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛\n")
        print("* O jogo realiza-se no seguinte tabuleiro, inicialmente vazio. Em cada jogada, cada jogador realiza uma das seguintes ações.\n")
        print("* Coloca uma peça verde num quadrado vazio;\n")
        print("* Substitui uma peça verde por uma peça amarela\n")
        print("* Substitui uma peça amarela por uma peça vermelha\n")
        print("* De notar que as peças vermelhas não podem ser substituídas. Isto significa que o jogo tem de terminar sempre: à medida que o tabuleiro fica com peças vermelhas, é inevitável que surja uma linha de três peças.\n")
        voltar()
        
def imprimir_matriz():

    print("\n") 
    print("\t    A     B     C     D")
    print("\t ┏━━━━━┳━━━━━┳━━━━━┳━━━━━┓")
    print("0:\t" + " ┃  " + str(matriz[0][0]) + "  ┃  " + str(matriz[0][1]) + "  ┃  " + str(matriz[0][2]) + "  ┃  " + str(matriz[0][3]) + "  ┃  " )
    print("\t ┣━━━━━╋━━━━━╋━━━━━╋━━━━━┫")
    print("1:\t" + " ┃  "+ str(matriz[1][0]) + "  ┃  " + str(matriz[1][1]) + "  ┃  " + str(matriz[1][2]) + "  ┃  " + str(matriz[1][3]) + "  ┃  " )
    print("\t ┣━━━━━╋━━━━━╋━━━━━╋━━━━━┫")
    print("2:\t" + " ┃  " + str(matriz[2][0]) + "  ┃  " + str(matriz[2][1]) + "  ┃  " + str(matriz[2][2]) + "  ┃  " + str(matriz[2][3]) + "  ┃ " )
    print("\t ┗━━━━━┻━━━━━┻━━━━━┻━━━━━┛")
    
def jogo(primeiro, segundo, jogar1, nome1, nome2, dificuldade):
    
    if jogar1 == 2:
        print("\nEscolha a dificuldade do BOT:")
        print("[1] Fácil        [2] Médio       [3] Dificil")
        dificuldade = int(input(""))
    elif jogar1 == 1:
        dificuldade = 0
        
    jogador_atual = primeiro
    vitoria = False
    res = 0
    res2 = 0

    if jogar1 == 1:
       
        while True:
         imprimir_matriz()
         vitoria = colocar(primeiro, segundo, jogador_atual, jogar1)
         if vitoria == True:
             break
        
    elif jogar1 == 2:
        if dificuldade == 1:
            while True:
                imprimir_matriz()
                jogador_atual, vitoria = jogada_bot(primeiro,segundo,jogador_atual)
                if vitoria == True:
                    break
        if dificuldade == 2:
            while True:
                imprimir_matriz()
                jogador_atual, vitoria = jogada_bot_medio(primeiro,segundo,jogador_atual)
                if vitoria == True:
                    break
        if dificuldade == 3:
            while True:
                imprimir_matriz()
                jogador_atual, vitoria = jogada_bot_dificil(primeiro,segundo,jogador_atual)
                if vitoria == True:
                    break
            
    print("\nDeseja voltar a jogar?")
    print("[1] Sim      [2] Não")
    res = int(input(""))
    if res == 1:
        res = jogar1
        for i in range(len(matriz)):
            for j in range(len(matriz[0])):
                matriz[i][j] = ' '
                
        primeiro = rd.randint(1,2)
        
        if primeiro == 1:
            print("\nO primeiro jogador é o " + nome1)
            print("O segundo jogador é o " + nome2)
            primeiro = nome1
            segundo = nome2
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2, dificuldade)
        else:
            print("\nO primeiro jogador é o " + nome2)
            print("O segundo jogador é o " + nome1)
            primeiro = nome2
            segundo = nome1
            dificuldade = 0
            jogo(primeiro, segundo, jogar1, nome1, nome2, dificuldade)
    else:
        print("\nDeseja voltar ao menu?")
        print("[1] Sim      [2] Não")
        res2 = int(input(""))
        for i in range(len(matriz)):
            for j in range(len(matriz[0])):
                matriz[i][j] = ' '
        if res2 == 1:
            menu()
        else: 
            return
   
def colocar(primeiro, segundo, jogador_atual, jogar1):
    
    print("\nDeseja passar a vez?")
    print("[1] Sim      [2] Não")
    passar = int(input(""))
    
    if passar == 1:
        if jogador_atual == primeiro:
            print(f"\nÉ a vez do(a) {segundo}")
            jogador_atual = segundo
            imprimir_matriz()
            colocar(primeiro, segundo, jogador_atual, jogar1)
        if jogador_atual == segundo:
            print(f"\nÉ a vez do(a) {primeiro}")
            jogador_atual = primeiro
            imprimir_matriz()
            colocar(primeiro, segundo, jogador_atual, jogar1)
            
    elif passar == 2:        
        
        l = int(input("\nLinha: "))
        c = int(input("Coluna: "))
    
        if l < 0 or l > 2 or c < 0 or c > 3:
            print("Posição inválida.\n")
            imprimir_matriz()
            colocar(primeiro, segundo, jogador_atual, jogar1)
        else:

            print("\nGreen [G]\nYellow [Y]\nRed [R]")
            cor = input("").upper()
        
            if cor == "G" and matriz[l][c] == " ":
                matriz[l][c] = cor
            elif cor == "Y" and matriz[l][c] == "G":
                matriz[l][c] = cor  
            elif cor == "R" and matriz[l][c] == "Y":
                matriz[l][c] = cor
            elif cor == "G" and (matriz[l][c] == "Y" or matriz[l][c] == "R"):
                print("\nNão pode colocar a cor verde nesta posição, tente novamente.")
                colocar(primeiro, segundo, jogador_atual)
            elif cor == "Y" and (matriz[l][c] == "R" or matriz[l][c] == " "):
                print("\nNão pode colocar a cor amarela nesta posição, tente novamente.") 
                colocar(primeiro, segundo, jogador_atual)
            elif cor == "R" and (matriz[l][c] == "G" or matriz[l][c] == " "):
                print("\nNão pode colocar a cor vermelho nesta posição, tente novamente")
                colocar(primeiro, segundo, jogador_atual)    
            else:
                print("Posição já preenchida.")
                colocar(primeiro, segundo, jogador_atual, jogar1)    
            if cor == "Y" and matriz[l][c] == "G":
                matriz[l][c] = cor   
                colocar(primeiro, segundo, jogador_atual) 
            
            vitoria, vencedor = verificar_vitoria(jogador_atual)
            if vitoria:
                print(f"\nO jogador {vencedor} venceu!")
                imprimir_matriz()
            elif jogador_atual == primeiro:
                print(f"\nÉ a vez do(a) {segundo}")
                jogador_atual = segundo
            elif jogador_atual == segundo:
                print(f"\nÉ a vez do(a) {primeiro}")
                jogador_atual = primeiro
        
            return jogador_atual, vitoria       
        
    else:
        print("\nInsira um valor válido!")
        imprimir_matriz()
        colocar(primeiro, segundo, jogador_atual, jogar1)
        
def verificar_vitoria(jogador_atual):
    for l in range(3):
        if matriz[l][0] == matriz[l][1] == matriz[l][2] != ' ':
            return True, jogador_atual
    for l in range(3):
        if matriz[l][1] == matriz[l][2] == matriz[l][3] != ' ':
            return True, jogador_atual

    for c in range(4):
        if matriz[0][c] == matriz[1][c] == matriz[2][c] != ' ':
            return True, jogador_atual

    if matriz[0][0] == matriz[1][1] == matriz[2][2] != ' ':
        return True, jogador_atual
    
    if matriz[0][1] == matriz[1][2] == matriz[2][3] != ' ':
        return True, jogador_atual
    
    if matriz[0][3] == matriz[1][2] == matriz[2][1] != ' ':
        return True, jogador_atual
    
    if matriz[0][2] == matriz[1][1] == matriz[2][0] != ' ':
        return True, jogador_atual

    if matriz[0][2] == matriz[1][1] == matriz[2][0] != ' ':
        return True, jogador_atual

    return False, None
   
def jogada_bot(primeiro, segundo, jogador_atual):
            
    if jogador_atual == "BOT":
        cor = ['G', 'Y', 'R']
        jogada_valida = False
    
        while not jogada_valida:
            l = rd.randint(0, 2)
            c = rd.randint(0, 3)

            if matriz[l][c] == " ":
                i = rd.randint(0, 2)
                if cor[i] == "G":
                    matriz[l][c] = cor[i]
                elif matriz[l][c] == "G" and cor[i] == "Y":
                    matriz[l][c] = cor[i]
                elif matriz[l][c] == "Y" and cor[i] == "R":
                    matriz[l][c] = cor[i]
                else:
                    continue
                jogada_valida = True

            elif matriz[l][c] == "G":
                i = rd.randint(0, 2)
                if cor[i] == "Y":
                    matriz[l][c] = cor[i]
                else:
                    continue
                jogada_valida = True

            elif matriz[l][c] == "Y":
                i = rd.randint(0, 2)
                if cor[i] == "R":
                    matriz[l][c] = cor[i]
                else:
                    continue 
                jogada_valida = True 
        
        print(f"\nLinha: {l}")
        print(f"Coluna: {c}") 
        pass   
      
    else:
        l = int(input("\nLinha: "))
        c = int(input("Coluna: "))
    
        if l < 0 or l > 2 or c < 0 or c > 3:
            imprimir_matriz()
            print("Posição inválida.")
            jogada_bot(primeiro, segundo, jogador_atual)
            
        print("\nGreen [G]\nYellow [Y]\nRed [R]")
        cor = input("").upper()
        
        if cor == "G" and matriz[l][c] == " ":
            matriz[l][c] = cor
        elif cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor  
        elif cor == "R" and matriz[l][c] == "Y":
            matriz[l][c] = cor
        elif cor == "G" and (matriz[l][c] == "Y" or matriz[l][c] == "R"):
            print("\nNão pode colocar a cor verde nesta posição, tente novamente.")
            jogada_bot(primeiro, segundo, jogador_atual)
        elif cor == "Y" and (matriz[l][c] == "R" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor amarela nesta posição, tente novamente.") 
            jogada_bot(primeiro, segundo, jogador_atual)
        elif cor == "R" and (matriz[l][c] == "G" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor vermelho nesta posição, tente novamente")
            jogada_bot(primeiro, segundo, jogador_atual)    
        else:
            print("Posição já preenchida.")
            jogada_bot(primeiro, segundo, jogador_atual)
        if cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor   
            jogada_bot(primeiro, segundo, jogador_atual) 
 
    
    vitoria, cor = verificar_vitoria(jogador_atual)
    if vitoria:
        print(f"\nO jogador {cor} venceu!")
        imprimir_matriz()
    elif jogador_atual == primeiro:
        print(f"\nÉ a vez do(a) {segundo}")
        jogador_atual = segundo
    elif jogador_atual == segundo:
        print(f"\nÉ a vez do(a) {primeiro}")
        jogador_atual = primeiro
    
    return jogador_atual, vitoria      

def jogada_bot_medio(primeiro, segundo, jogador_atual):
    
    if jogador_atual == "BOT":
        cor = ['G', 'Y', 'R']
        jogada_valida = False

        for l in range(3):
            for c in range(4):
                if matriz[l][c] == " ":
                    for i in range(3):
                        if cor[i] == "G":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        elif matriz[l][c] == "G" and cor[i] == "Y":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        elif matriz[l][c] == "Y" and cor[i] == "R":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        if jogada_valida:
                            break

                    if jogada_valida:
                        break

                if jogada_valida:
                    break

            if jogada_valida:
                break

        if not jogada_valida:
            while not jogada_valida:
                l = rd.randint(0, 2)
                c = rd.randint(0, 3)

                if matriz[l][c] == " ":
                    i = rd.randint(0, 2)
                    if cor[i] == "G":
                        matriz[l][c] = cor[i]
                    elif matriz[l][c] == "G" and cor[i] == "Y":
                        matriz[l][c] = cor[i]
                    elif matriz[l][c] == "Y" and cor[i] == "R":
                        matriz[l][c] = cor[i]
                    else:
                        continue
                    jogada_valida = True

                elif matriz[l][c] == "G":
                    i = rd.randint(0, 2)
                    if cor[i] == "Y":
                        matriz[l][c] = cor[i]
                    else:
                        continue
                    jogada_valida = True

                elif matriz[l][c] == "Y":
                    i = rd.randint(0, 2)
                    if cor[i] == "R":
                        matriz[l][c] = cor[i]
                    else:
                        continue 
                    jogada_valida = True 

                if verificar_vitoria("BOT")[0]:
                    print("O BOT ganhou!")
                    break

        if verificar_vitoria("BOT")[0]:
            print("O BOT ganhou!")
            vitoria, cor = verificar_vitoria(jogador_atual)
            if vitoria:
                print(f"\nO jogador {cor} venceu!")
                imprimir_matriz()
            return jogador_atual, vitoria
        else:
            print(f"\nLinha: {l}")
            print(f"Coluna: {c}")

 
      
    else:
        l = int(input("\nLinha: "))
        c = int(input("Coluna: "))
    
        if l < 0 or l > 2 or c < 0 or c > 3:
            imprimir_matriz()
            print("Posição inválida.")
            jogada_bot_medio(primeiro, segundo, jogador_atual)
            
        print("\nGreen [G]\nYellow [Y]\nRed [R]")
        cor = input("").upper()
        
        if cor == "G" and matriz[l][c] == " ":
            matriz[l][c] = cor
        elif cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor  
        elif cor == "R" and matriz[l][c] == "Y":
            matriz[l][c] = cor
        elif cor == "G" and (matriz[l][c] == "Y" or matriz[l][c] == "R"):
            print("\nNão pode colocar a cor verde nesta posição, tente novamente.")
            jogada_bot_medio(primeiro, segundo, jogador_atual)
        elif cor == "Y" and (matriz[l][c] == "R" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor amarela nesta posição, tente novamente.") 
            jogada_bot_medio(primeiro, segundo, jogador_atual)
        elif cor == "R" and (matriz[l][c] == "G" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor vermelho nesta posição, tente novamente")
            jogada_bot_medio(primeiro, segundo, jogador_atual)    
        else:
            print("Posição já preenchida.")
            jogada_bot_medio(primeiro, segundo, jogador_atual)
        if cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor   
            jogada_bot_medio(primeiro, segundo, jogador_atual) 
 
    
    vitoria, vencedor = verificar_vitoria(jogador_atual)
    if vitoria:
        print(f"\nO jogador {vencedor} venceu!")
        imprimir_matriz()
    elif jogador_atual == primeiro:
        print(f"\nÉ a vez do(a) {segundo}")
        jogador_atual = segundo
    elif jogador_atual == segundo:
        print(f"\nÉ a vez do(a) {primeiro}")
        jogador_atual = primeiro
    
    return jogador_atual, vitoria  

def jogada_bot_dificil(primeiro, segundo, jogador_atual):
    
    if jogador_atual == "BOT":
        cor = ['G', 'Y', 'R']
        jogada_valida = False

        for l in range(3):
            for c in range(4):
                if matriz[l][c] == " ":
                    for i in range(3):
                        if cor[i] == "G":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        elif matriz[l][c] == "G" and cor[i] == "Y":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        elif matriz[l][c] == "Y" and cor[i] == "R":
                            matriz[l][c] = cor[i]
                            if verificar_vitoria("BOT")[0]:
                                jogada_valida = True
                                break
                            matriz[l][c] = " "
                        if jogada_valida:
                            break

                    if jogada_valida:
                        break

                if jogada_valida:
                    break

            if jogada_valida:
                break
        
        
        for l in range(3):
            for c in range(4):
                if matriz[l][c] == " ":
                    if jogador_atual == primeiro and primeiro == "BOT":
                        jogador_atual = segundo
                    elif jogador_atual == segundo and segundo == "BOT":
                        jogador_atual = primeiro
                    else:
                        jogador_atual = "BOT"
                    matriz[l][c] = cor[0] if jogador_atual == "BOT" else cor[2]
                    if verificar_vitoria(jogador_atual)[0]:
                        matriz[l][c] = " "
                        continue
                    matriz[l][c] = " "
                    matriz[l][c] = cor[0] if jogador_atual != "BOT" else cor[2]
                    if verificar_vitoria(jogador_atual)[0]:
                        matriz[l][c] = " "
                        return l, c
                    matriz[l][c] = " "

        if jogada_valida:       
            if jogador_atual == primeiro and primeiro == "BOT":
                jogador_atual = segundo
            elif jogador_atual == segundo and segundo == "BOT":
                jogador_atual = primeiro
                     
        if jogada_valida == False:
            for l in range(3):
                for c in range(4):
                    if matriz[l][c] == " ":
                        for i in range(3):
                            if cor[i] == "R":
                                matriz[l][c] = cor[i]
                                if verificar_vitoria(jogador_atual)[0]:
                                    matriz[l][c] = " "
                                    return l, c
                                elif verificar_vitoria(jogador_atual == 2 and matriz[l][c+1] == matriz[l][c+2] == "Y"):
                                    matriz[l][c] = " "
                                    break
                                elif verificar_vitoria(jogador_atual == 2 and matriz[l+1][c] == matriz[l+2][c] == "Y"):
                                    matriz[l][c] = " "
                                    break
                                elif verificar_vitoria(jogador_atual == 2 and (matriz[l][c+1] == matriz[l][c+2] == "G" or matriz[l][c-1] == matriz[l][c+1] == "G" or matriz[l][c-2] == matriz[l][c-1] == "G")):
                                    matriz[l][c] = " "
                                    break
                                elif verificar_vitoria(jogador_atual == 2 and (matriz[l+1][c] == matriz[l+2][c] == "G " or matriz[l-1][c] == matriz[l+1][c] == "G" or matriz[l-1][c] == matriz[l-2][c] == "G")):
                                    matriz[l][c] = " "
                                    break
                                matriz[l][c] = " "
                            elif cor[i] == "Y":
                                matriz[l][c] = cor[i]
                                if verificar_vitoria(jogador_atual)[0]:
                                    matriz[l][c] = " "
                                    return l, c
                                elif verificar_vitoria(jogador_atual == 2 and matriz[l][c+1] == matriz[l][c+2] == "R"):
                                    matriz[l][c] = " "
                                    break
                                elif verificar_vitoria(jogador_atual == 2 and (matriz[l][c+1] == matriz[l][c+2] == "G" or matriz[l][c-1] == matriz[l][c+1] == "G" or matriz[l][c-2] == matriz[l][c-1] == "G")):
                                    matriz[l][c] = " "
                                    break
                                matriz[l][c] = " "
                            elif cor[i] == "G":
                                matriz[l][c] = cor[i]
                                if verificar_vitoria(jogador_atual)[0]:
                                    matriz[l][c] = " "
                                    return l, c
                                elif verificar_vitoria(jogador_atual == 2 and (matriz[l][c+1] == matriz[l][c+2] == "G" or matriz[l][c-1] == matriz[l][c+1] == "G" or matriz[l][c-2] == matriz[l][c-1] == "G")):
                                    matriz[l][c] = " "
                                    break
                                elif verificar_vitoria(jogador_atual == 2 and (matriz[l+1][c] == matriz[l+2][c] == "G" or matriz[l-1][c] == matriz[l+1][c] == "G" or matriz[l-2][c] == matriz[l-1][c] == "G")):
                                    matriz[l][c] = " "
                                    break
                                matriz[l][c] = " "                               

          
        if not jogada_valida and jogador_atual != "BOT":
            jogador_atual = "BOT"
       
        if not jogada_valida and jogador_atual == "BOT":
            while not jogada_valida:
                l = rd.randint(0, 2)
                c = rd.randint(0, 3)

                if matriz[l][c] == " ":
                    i = rd.randint(0, 2)
                    if cor[i] == "G":
                        matriz[l][c] = cor[i]
                        break
                    elif matriz[l][c] == "G" and cor[i] == "Y":
                        matriz[l][c] = cor[i]
                        break
                    elif matriz[l][c] == "Y" and cor[i] == "R":
                        matriz[l][c] = cor[i]
                        break
                    else:
                        continue
                    
                if verificar_vitoria("BOT")[0]:
                    if (c > 0 and matriz[l][c-1] == jogador_atual) or (c < 3 and matriz[l][c+1] == jogador_atual):
                        matriz[l][c] = " "  
                        continue
                    jogada_valida = True

                elif matriz[l][c] == "G":
                    i = rd.randint(0, 2)
                    if cor[i] == "Y":
                        matriz[l][c] = cor[i]
                    else:
                        continue
                    jogada_valida = True

                elif matriz[l][c] == "Y":
                    i = rd.randint(0, 2)
                    if cor[i] == "R":
                        matriz[l][c] = cor[i]
                    else:
                        continue 
                    jogada_valida = True 

                if verificar_vitoria("BOT")[0]:
                    print("O BOT ganhou!")
                    break

        if verificar_vitoria("BOT")[0]:
            print(f"\nLinha: {l}")
            print(f"Coluna: {c}")
            print("O BOT ganhou!")
            vitoria, cor = verificar_vitoria(jogador_atual)
            if vitoria:
                imprimir_matriz()
            return jogador_atual, vitoria
        else:
            print(f"\nLinha: {l}")
            print(f"Coluna: {c}") 
        pass
        
      
    else:
        l = int(input("\nLinha: "))
        c = int(input("Coluna: "))
    
        if l < 0 or l > 2 or c < 0 or c > 3:
            imprimir_matriz()
            print("Posição inválida.")
            jogada_bot_dificil(primeiro, segundo, jogador_atual)
            
        print("\nGreen [G]\nYellow [Y]\nRed [R]")
        cor = input("").upper()
        
        if cor == "G" and matriz[l][c] == " ":
            matriz[l][c] = cor
        elif cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor  
        elif cor == "R" and matriz[l][c] == "Y":
            matriz[l][c] = cor
        elif cor == "G" and (matriz[l][c] == "Y" or matriz[l][c] == "R"):
            print("\nNão pode colocar a cor verde nesta posição, tente novamente.")
            jogada_bot_dificil(primeiro, segundo, jogador_atual)
        elif cor == "Y" and (matriz[l][c] == "R" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor amarela nesta posição, tente novamente.") 
            jogada_bot_dificil(primeiro, segundo, jogador_atual)
        elif cor == "R" and (matriz[l][c] == "G" or matriz[l][c] == " "):
            print("\nNão pode colocar a cor vermelho nesta posição, tente novamente")
            jogada_bot_dificil(primeiro, segundo, jogador_atual)    
        else:
            print("Posição já preenchida.")
            jogada_bot_dificil(primeiro, segundo, jogador_atual)
        if cor == "Y" and matriz[l][c] == "G":
            matriz[l][c] = cor   
            jogada_bot_dificil(primeiro, segundo, jogador_atual) 
 
    
    vitoria, vencedor = verificar_vitoria(jogador_atual)
    if vitoria:
        print(f"\nO jogador {vencedor} venceu!")
        imprimir_matriz()
    elif jogador_atual == primeiro:
        print(f"\nÉ a vez do(a) {segundo}")
        jogador_atual = segundo
    elif jogador_atual == segundo:
        print(f"\nÉ a vez do(a) {primeiro}")
        jogador_atual = primeiro
    
    return jogador_atual, vitoria  

menu()