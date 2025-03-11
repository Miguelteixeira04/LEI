import pygame
import random
import time

pygame.init()
random.seed()

screen_width = 1280 
screen_height = 720 
screen = pygame.display.set_mode((screen_width, screen_height))
pygame.display.set_caption("JOGO DO SEMÁFORO")

# define cores
BRANCO = (255, 255, 255)
PRETO = (0, 0, 0)
CINZA = (68, 68, 68)

# import pagina inicial
pagina_inicial = pygame.image.load('pagina_inicial.png')
pagina_inicial_redim = pygame.transform.scale(pagina_inicial, (1280, 720)) 
# import fundo menu inicial
menu_inicial = pygame.image.load('menu_jogo.png')
menu_redim = pygame.transform.scale(menu_inicial, (1280, 720))
# import fundo comecar
menu_comecar = pygame.image.load('menu_comecar.png')
menu_comecar_redim = pygame.transform.scale(menu_comecar, (1280, 720))
# import fundo regras
menu_regras= pygame.image.load('menu_regras.png')
menu_regras_redim = pygame.transform.scale(menu_regras, (1280, 720))
# import fundo nomes
menu_nomes = pygame.image.load("menu_nomes.png")
menu_nomes_redim = pygame.transform.scale(menu_nomes, (1280, 720))
# import fundo nome com bot
menu_nomebot = pygame.image.load("menu_nomebot.png")
menu_nomebot_redim = pygame.transform.scale(menu_nomebot, (1280, 720))
# import fundo tabuleiro 
menu_tabuleiro = pygame.image.load("menu_tabuleiro.png")
menu_tabuleiro_redim = pygame.transform.scale(menu_tabuleiro, (1280, 720))
# import fundo vitoria
menu_vitoria = pygame.image.load("menu_vitoria.png")
menu_vitoria_redim = pygame.transform.scale(menu_vitoria, (1280, 720))
# import fundo 1vbot
fundo_tabuleiro = pygame.image.load('menu_tabuleiro.png')
fundo_tabuleiro_redim = pygame.transform.scale(fundo_tabuleiro, (1280, 720))

# import label nome1
label_nome1 = pygame.image.load('nome1.png')
label_nome1_redim = pygame.transform.scale(label_nome1, (332,77))
# import label nome2
label_nome2 = pygame.image.load('nome2.png')
label_nome2_redim = pygame.transform.scale(label_nome2, (332,77))

# import botao sair
botao_sair = pygame.image.load('sair.png')
sair_redim = pygame.transform.scale(botao_sair, (236,101))
# import botao comecar
botao_comecar = pygame.image.load('comecar.png')
comecar_redim = pygame.transform.scale(botao_comecar, (281,106))
# import botao carregar
botao_carregar = pygame.image.load('carregar.png')
carregar_redim = pygame.transform.scale(botao_carregar, (281,110))
# import botao regras
botao_regras = pygame.image.load('regras.png')
regras_redim = pygame.transform.scale(botao_regras, (281,106))
# import botao 1v1
botao_1v1 = pygame.image.load('1v1.png')
botao_1v1_redim = pygame.transform.scale(botao_1v1, (352,184))
# import botao 1vbot
botao_1vbot = pygame.image.load('1vbot.png')
botao_1vbot_redim = pygame.transform.scale(botao_1vbot, (352,194))
# import botao voltar
botao_voltar = pygame.image.load('voltar.png')
botao_voltar_redim = pygame.transform.scale(botao_voltar, (73,54))
# import botao menu
botao_menu = pygame.image.load('menu.png')
botao_menu_redim = pygame.transform.scale(botao_menu, (236,101))

# import botoes tabuleiro
botao_tabuleiro_1_1 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_1_1_redim = pygame.transform.scale(botao_tabuleiro_1_1, (163, 133))
botao_tabuleiro_1_2 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_1_2_redim = pygame.transform.scale(botao_tabuleiro_1_2, (163, 133))
botao_tabuleiro_1_3 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_1_3_redim = pygame.transform.scale(botao_tabuleiro_1_1, (163, 133))
botao_tabuleiro_1_4 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_1_4_redim = pygame.transform.scale(botao_tabuleiro_1_4, (163, 133))
botao_tabuleiro_2_1 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_2_1_redim = pygame.transform.scale(botao_tabuleiro_2_1, (163, 133))
botao_tabuleiro_2_2 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_2_2_redim = pygame.transform.scale(botao_tabuleiro_2_2, (163, 133))
botao_tabuleiro_2_3 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_2_3_redim = pygame.transform.scale(botao_tabuleiro_2_3, (163, 133))
botao_tabuleiro_2_4 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_2_4_redim = pygame.transform.scale(botao_tabuleiro_2_4, (163, 133))
botao_tabuleiro_3_1 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_3_1_redim = pygame.transform.scale(botao_tabuleiro_3_1, (163, 133))
botao_tabuleiro_3_2 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_3_2_redim = pygame.transform.scale(botao_tabuleiro_3_2, (163, 133))
botao_tabuleiro_3_3 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_3_3_redim = pygame.transform.scale(botao_tabuleiro_3_3, (163, 133))                                      
botao_tabuleiro_3_4 = pygame.image.load('botao_tabuleiro.png')
botao_tabuleiro_3_4_redim = pygame.transform.scale(botao_tabuleiro_3_4, (163, 133))

# import peças do jogo
circulo_redim = pygame.image.load('circulo.png')
circulo_redim = pygame.transform.scale(circulo_redim, (125, 125))
triangulo_redim = pygame.image.load('triangulo.png')
triangulo_redim = pygame.transform.scale(triangulo_redim, (150, 150))
quadrado_redim = pygame.image.load('quadrado.png')
quadrado_redim = pygame.transform.scale(quadrado_redim, (150, 150))

# mostrar as regras do jogo
def abrir_janela_regras():
    screen = pygame.display.set_mode((screen_width, screen_height))
    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_voltar_redim.get_rect(topleft=(1200,5)).collidepoint(mouse_pos):
                    running = False

        screen.blit(menu_regras_redim, (0,0))
        screen.blit(botao_voltar_redim, (1200, 5))
        pygame.display.update()

# selecionar 1vs1 ou 1vsbot
def abrir_janela_comecar():
    screen = pygame.display.set_mode((screen_width, screen_height))
    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_1v1_redim.get_rect(topleft=(150,300)).collidepoint(mouse_pos):
                    abrir_janela_nomes_1v1()
                elif botao_1vbot_redim.get_rect(topleft=(800,295)).collidepoint(mouse_pos):
                    abrir_janela_nomes_1vbot()
                elif botao_voltar_redim.get_rect(topleft=(1200,5)).collidepoint(mouse_pos):
                    running = False
    
        screen.blit(menu_comecar_redim, (0,0))
        screen.blit(botao_1v1_redim, (150, 300))
        screen.blit(botao_1vbot_redim, (800, 295))
        screen.blit(botao_voltar_redim, (1200, 5))
        pygame.display.update()

# inserir o nome dos jogadores 1v1
def abrir_janela_nomes_1v1():
    screen = pygame.display.set_mode((screen_width, screen_height))
    jogador_atual = 1
    nome_jogador1 = ""
    nome_jogador2 = ""

    digitando = True
    while digitando:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                digitando = False
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_RETURN: # clicar no enter para passar ao nome 2
                    if jogador_atual == 1:
                        jogador_atual = 2
                    else:
                        abrir_tabuleiro_1v1(nome_jogador1[:12], nome_jogador2[:12])  # clicar 2a vez no enter para avançar para o tabuleiro
                elif event.key == pygame.K_BACKSPACE: # apagar caracteres 1 a 1
                    if jogador_atual == 1:
                        nome_jogador1 = nome_jogador1[:-1]
                    else:
                        nome_jogador2 = nome_jogador2[:-1]
                else:
                    if jogador_atual == 1:
                        if len(nome_jogador1) < 12:
                            nome_jogador1 += event.unicode # adiciona cara caracter ao nome enquanto foir <12
                    else:
                        if len(nome_jogador2) < 12:
                            nome_jogador2 += event.unicode
            elif event.type == pygame.MOUSEBUTTONDOWN: # apaga o nome se clicar por cima dele  
                mouse_pos = pygame.mouse.get_pos()       
                if event.button == 1:
                    if posicao_texto_jogador1.collidepoint(event.pos):
                        jogador_atual = 1
                        nome_jogador1 = ""
                    elif posicao_texto_jogador2.collidepoint(event.pos):
                        jogador_atual = 2
                        nome_jogador2 = ""
                    elif botao_voltar_redim.get_rect(topleft=(1200,5)).collidepoint(mouse_pos):
                        digitando = False

        screen.blit(menu_nomes_redim, (0, 0))
        screen.blit(botao_voltar_redim, (1200, 5))

        fonte = pygame.font.Font(None, 46)
        texto_jogador1 = fonte.render(nome_jogador1, True, BRANCO)
        texto_jogador2 = fonte.render(nome_jogador2, True, BRANCO)
        posicao_texto_jogador1 = texto_jogador1.get_rect(midleft=(screen_width // 2 - 525, screen_height // 2 + 48))
        posicao_texto_jogador2 = texto_jogador2.get_rect(midleft=(screen_width // 2 + 155, screen_height // 2 + 48))
        pygame.draw.rect(screen, CINZA, posicao_texto_jogador1)
        pygame.draw.rect(screen, CINZA, posicao_texto_jogador2)
        screen.blit(texto_jogador1, posicao_texto_jogador1)
        screen.blit(texto_jogador2, posicao_texto_jogador2)

        pygame.display.update()

# inserir o nome do jogador 1vbot
def abrir_janela_nomes_1vbot():
    screen = pygame.display.set_mode((screen_width, screen_height))
    jogador_atual = 1
    nome_jogador = ""

    digitando = True
    while digitando:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return False
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_RETURN:
                    abrir_tabuleiro_1vbot_facil(nome_jogador[:12])
                elif event.key == pygame.K_BACKSPACE:
                    nome_jogador = nome_jogador[:-1]
                else:
                    if len(nome_jogador) < 12:
                        nome_jogador += event.unicode
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if event.button == 1:
                    if posicao_texto_jogador.collidepoint(event.pos):
                        jogador_atual = 1
                        nome_jogador = ""
                    elif botao_voltar_redim.get_rect(topleft=(1200, 5)).collidepoint(mouse_pos):
                        return True

        screen.blit(menu_nomebot_redim, (0, 0))
        screen.blit(botao_voltar_redim, (1200, 5))

        # Exibindo o nome digitado na janela
        fonte = pygame.font.Font(None, 46)
        texto_jogador = fonte.render(nome_jogador, True, BRANCO)
        posicao_texto_jogador = texto_jogador.get_rect(midleft=(screen_width // 2 - 525, screen_height // 2 + 48))
        # Desenhar retângulo interativo para reescrever o nome
        pygame.draw.rect(screen, CINZA, posicao_texto_jogador)
        screen.blit(texto_jogador, posicao_texto_jogador)
        
        pygame.display.update()

# jogo 1vsbot
def abrir_tabuleiro_1vbot_facil(nome_jogador):
    screen = pygame.display.set_mode((screen_width, screen_height))
    nome2 = 'BOT'
    jogadores = [nome_jogador, nome2]
    nome_ou_bot_selecionado = random.choice(jogadores)
    jogador_selecionado = random.randint(1,2)
    jogador_atual = 1

    if jogador_selecionado == 1:
        jogador_atual = 1
        nome_ou_bot_selecionado = nome_jogador
    else:
        jogador_atual = 2
        nome_ou_bot_selecionado = nome2

    botao_vazio_redim = pygame.Surface((125, 125))
    botao_vazio_redim.fill((166, 124, 74))
    
    imagem_botao_1_1 = botao_vazio_redim
    imagem_botao_1_2 = botao_vazio_redim
    imagem_botao_1_3 = botao_vazio_redim
    imagem_botao_1_4 = botao_vazio_redim 
    imagem_botao_2_1 = botao_vazio_redim
    imagem_botao_2_2 = botao_vazio_redim
    imagem_botao_2_3 = botao_vazio_redim
    imagem_botao_2_4 = botao_vazio_redim
    imagem_botao_3_1 = botao_vazio_redim
    imagem_botao_3_2 = botao_vazio_redim
    imagem_botao_3_3 = botao_vazio_redim
    imagem_botao_3_4 = botao_vazio_redim

    jogador_atual = nome_ou_bot_selecionado 
    p1 = True
    p2 = True

    digitando = True
    while digitando:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                digitando = False
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if sair_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    pygame.quit()
                    quit() 
                elif botao_voltar_redim.get_rect(topleft=(1200, 5)).collidepoint(mouse_pos):
                    digitando = False
                
                if jogador_atual == nome_jogador: # Verifica se é a vez do player jogar
                    p1 = True
                    p2 = False

                    if botao_tabuleiro_1_1_redim.get_rect(topleft=(80, 214)).collidepoint(mouse_pos):
                        if imagem_botao_1_1 == botao_vazio_redim:
                            imagem_botao_1_1 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_1 == circulo_redim:
                            imagem_botao_1_1 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_1 == triangulo_redim:
                            imagem_botao_1_1 = quadrado_redim
                            jogador_atual = nome2
                               
                    elif botao_tabuleiro_1_2_redim.get_rect(topleft=(254, 214)).collidepoint(mouse_pos):
                        if imagem_botao_1_2 == botao_vazio_redim:
                            imagem_botao_1_2 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_2 == circulo_redim:
                            imagem_botao_1_2 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_2 == triangulo_redim:
                            imagem_botao_1_2 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_1_3_redim.get_rect(topleft=(428, 214)).collidepoint(mouse_pos):       
                        if imagem_botao_1_3 == botao_vazio_redim:
                            imagem_botao_1_3 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_3 == circulo_redim:
                            imagem_botao_1_3 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_3 == triangulo_redim:
                            imagem_botao_1_3 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_1_4_redim.get_rect(topleft=(602, 214)).collidepoint(mouse_pos):                        
                        if imagem_botao_1_4 == botao_vazio_redim:
                            imagem_botao_1_4 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_4 == circulo_redim:
                            imagem_botao_1_4 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_1_4 == triangulo_redim:
                            imagem_botao_1_4 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_2_1_redim.get_rect(topleft=(80, 358)).collidepoint(mouse_pos):                       
                        if imagem_botao_2_1 == botao_vazio_redim:
                            imagem_botao_2_1 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_1 == circulo_redim:
                            imagem_botao_2_1 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_1 == triangulo_redim:
                            imagem_botao_2_1 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_2_2_redim.get_rect(topleft=(254, 358)).collidepoint(mouse_pos):                       
                        if imagem_botao_2_2 == botao_vazio_redim:
                            imagem_botao_2_2 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_2 == circulo_redim:
                            imagem_botao_2_2 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_2 == triangulo_redim:
                            imagem_botao_2_2 = quadrado_redim
                            jogador_atual = nome2
                               
                    elif botao_tabuleiro_2_3_redim.get_rect(topleft=(428, 358)).collidepoint(mouse_pos):                        
                        if imagem_botao_2_3 == botao_vazio_redim:
                            imagem_botao_2_3 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_3 == circulo_redim:
                            imagem_botao_2_3 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_3 == triangulo_redim:
                            imagem_botao_2_3 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_2_4_redim.get_rect(topleft=(602, 358)).collidepoint(mouse_pos):                      
                        if imagem_botao_2_4 == botao_vazio_redim:
                            imagem_botao_2_4 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_4 == circulo_redim:
                            imagem_botao_2_4 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_2_4 == triangulo_redim:
                            imagem_botao_2_4 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_3_1_redim.get_rect(topleft=(80, 502)).collidepoint(mouse_pos):                       
                        if imagem_botao_3_1 == botao_vazio_redim:
                            imagem_botao_3_1 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_1 == circulo_redim:
                            imagem_botao_3_1 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_1 == triangulo_redim:
                            imagem_botao_3_1 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_3_2_redim.get_rect(topleft=(254, 502)).collidepoint(mouse_pos):                       
                        if imagem_botao_3_2 == botao_vazio_redim:
                            imagem_botao_3_2 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_2 == circulo_redim:
                            imagem_botao_3_2 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_2 == triangulo_redim:
                            imagem_botao_3_2 = quadrado_redim
                            jogador_atual = nome2
                                
                    elif botao_tabuleiro_3_3_redim.get_rect(topleft=(428, 502)).collidepoint(mouse_pos):                       
                        if imagem_botao_3_3 == botao_vazio_redim:
                            imagem_botao_3_3 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_3 == circulo_redim:
                            imagem_botao_3_3 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_3 == triangulo_redim:
                            imagem_botao_3_3 = quadrado_redim
                            jogador_atual = nome2
                               
                    elif botao_tabuleiro_3_4_redim.get_rect(topleft=(602, 502)).collidepoint(mouse_pos):                       
                        if imagem_botao_3_4 == botao_vazio_redim:
                            imagem_botao_3_4 = circulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_4 == circulo_redim:
                            imagem_botao_3_4 = triangulo_redim
                            jogador_atual = nome2
                        elif imagem_botao_3_4 == triangulo_redim:
                            imagem_botao_3_4 = quadrado_redim
                            jogador_atual = nome2

                    p1 = False
                    p2 = True

            #bot
            elif jogador_atual == nome2:  # Verifica se é a vez do bot jogar
                time.sleep(0.25)
                p1 = False 
                p2 = True 
                opcoes_linha = ('1','2','3')
                opcoes_coluna = ('A','B','C','D')
                opcao_lin = random.choice(opcoes_linha)
                opcao_col = random.choice(opcoes_coluna)
                casa =  opcao_col + opcao_lin

                if casa == 'A1':
                    if imagem_botao_1_1 == botao_vazio_redim:
                        screen.blit(circulo_redim,(100, 217))
                        imagem_botao_1_1 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_1_1 == circulo_redim :
                        screen.blit(triangulo_redim,(86, 205))
                        imagem_botao_1_1 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif(imagem_botao_1_1 == triangulo_redim):
                        screen.blit(quadrado_redim,(87, 205))
                        imagem_botao_1_1 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'B1':
                    if imagem_botao_1_2 == botao_vazio_redim:
                        screen.blit(circulo_redim,(274, 217))
                        imagem_botao_1_2 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_1_2 == circulo_redim:
                        screen.blit(triangulo_redim,(260, 205))
                        imagem_botao_1_2 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif(imagem_botao_1_2 == triangulo_redim):
                        screen.blit(quadrado_redim,(261, 205))
                        imagem_botao_1_2 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'C1':
                    if imagem_botao_1_3 == botao_vazio_redim:
                        screen.blit(circulo_redim,(448, 217))
                        imagem_botao_1_3 = circulo_redim
                        jogador_atual = nome_jogador
                    elif(imagem_botao_1_3 == circulo_redim):
                        screen.blit(triangulo_redim,(434, 205))
                        imagem_botao_1_3 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif(imagem_botao_1_3 == triangulo_redim):
                        screen.blit(quadrado_redim,(435, 205))
                        imagem_botao_1_3 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'D1':
                    if imagem_botao_1_4 == botao_vazio_redim:
                        screen.blit(circulo_redim, (622, 217))
                        imagem_botao_1_4 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_1_4 == circulo_redim:
                        screen.blit(triangulo_redim, (608, 205))
                        imagem_botao_1_4 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_1_4 == triangulo_redim:
                        screen.blit(quadrado_redim, (609, 205))
                        imagem_botao_1_4 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'A2':
                    if imagem_botao_2_1 == botao_vazio_redim:
                        screen.blit(circulo_redim, (100, 361))
                        imagem_botao_2_1 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_1 == circulo_redim:
                        screen.blit(triangulo_redim, (86, 349))
                        imagem_botao_2_1 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_1 == triangulo_redim:
                        screen.blit(quadrado_redim, (87, 349))
                        imagem_botao_2_1 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'B2':
                    if imagem_botao_2_2 == botao_vazio_redim:
                        screen.blit(circulo_redim, (274, 361))
                        imagem_botao_2_2 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_2 == circulo_redim:
                        screen.blit(triangulo_redim, (260, 349))
                        imagem_botao_2_2 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_2 == triangulo_redim:
                        screen.blit(quadrado_redim, (261, 349))
                        imagem_botao_2_2 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'C2':
                    if imagem_botao_2_3 == botao_vazio_redim:
                        screen.blit(circulo_redim, (448, 361))
                        imagem_botao_2_3 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_3 == circulo_redim:
                        screen.blit(triangulo_redim, (434, 349))
                        imagem_botao_2_3 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_3 == triangulo_redim:
                        screen.blit(quadrado_redim, (435, 349))
                        imagem_botao_2_3 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'D2':
                    if imagem_botao_2_4 == botao_vazio_redim:
                        screen.blit(circulo_redim, (622, 361))
                        imagem_botao_2_4 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_4 == circulo_redim:
                        screen.blit(triangulo_redim, (608, 349))
                        imagem_botao_2_4 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_2_4 == triangulo_redim:
                        screen.blit(quadrado_redim, (609, 349))
                        imagem_botao_2_4 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'A3':
                    if imagem_botao_3_1 == botao_vazio_redim:
                        screen.blit(circulo_redim, (100, 505))
                        imagem_botao_3_1 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_1 == circulo_redim:
                        screen.blit(triangulo_redim, (86, 493))
                        imagem_botao_3_1 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_1 == triangulo_redim:
                        screen.blit(quadrado_redim, (87, 493))
                        imagem_botao_3_1 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'B3':
                    if imagem_botao_3_2 == botao_vazio_redim:
                        screen.blit(circulo_redim, (274, 505))
                        imagem_botao_3_2 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_2 == circulo_redim:
                        screen.blit(triangulo_redim, (260, 493))
                        imagem_botao_3_2 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_2 == triangulo_redim:
                        screen.blit(quadrado_redim, (261, 493))
                        imagem_botao_3_2 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'C3':
                    if imagem_botao_3_3 == botao_vazio_redim:
                        screen.blit(circulo_redim, (488, 505))
                        imagem_botao_3_3 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_3 == circulo_redim:
                        screen.blit(triangulo_redim, (434, 493))
                        imagem_botao_3_3 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_3 == triangulo_redim:
                        screen.blit(quadrado_redim, (435, 493))
                        imagem_botao_3_3 = quadrado_redim
                        jogador_atual = nome_jogador
                
                if casa == 'D4':
                    if imagem_botao_3_4 == botao_vazio_redim:
                        screen.blit(circulo_redim, (602, 505))
                        imagem_botao_3_4 = circulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_4 == circulo_redim:
                        screen.blit(triangulo_redim, (608, 493))
                        imagem_botao_3_4 = triangulo_redim
                        jogador_atual = nome_jogador
                    elif imagem_botao_3_4 == triangulo_redim:
                        screen.blit(quadrado_redim, (609, 493))
                        imagem_botao_3_4 = quadrado_redim
                        jogador_atual = nome_jogador
                
                p1 = True
                p2 = False
                
        screen.blit(botao_voltar_redim, (1200, 5))
        screen.blit(fundo_tabuleiro_redim, (0, 0))

        screen.blit(botao_tabuleiro_1_1_redim, (80, 214))
        screen.blit(botao_tabuleiro_1_2_redim, (254, 214))
        screen.blit(botao_tabuleiro_1_3_redim, (428, 214))
        screen.blit(botao_tabuleiro_1_4_redim, (602, 214))

        screen.blit(botao_tabuleiro_2_1_redim, (80, 358))
        screen.blit(botao_tabuleiro_2_2_redim, (254, 358))
        screen.blit(botao_tabuleiro_2_3_redim, (428, 358))
        screen.blit(botao_tabuleiro_2_4_redim, (602, 358))

        screen.blit(botao_tabuleiro_3_1_redim, (80, 502))
        screen.blit(botao_tabuleiro_3_2_redim, (254, 502))
        screen.blit(botao_tabuleiro_3_3_redim, (428, 502))
        screen.blit(botao_tabuleiro_3_4_redim, (602, 502))

        # Compor estetica/posiçao das peças nos botoes
        if imagem_botao_1_1 == circulo_redim:
            screen.blit(imagem_botao_1_1, (100, 217))
        elif imagem_botao_1_1 == triangulo_redim:
            screen.blit(imagem_botao_1_1, (86, 205))
        elif imagem_botao_1_1 == quadrado_redim:
            screen.blit(imagem_botao_1_1, (87, 205))

        if imagem_botao_1_2 == circulo_redim:
            screen.blit(imagem_botao_1_2, (274, 217))
        elif imagem_botao_1_2 == triangulo_redim:
            screen.blit(imagem_botao_1_2, (260, 205))
        elif imagem_botao_1_2 == quadrado_redim:
            screen.blit(imagem_botao_1_2, (261, 205))

        if imagem_botao_1_3 == circulo_redim:
            screen.blit(imagem_botao_1_3, (448, 217))
        elif imagem_botao_1_3 == triangulo_redim:
            screen.blit(imagem_botao_1_3, (434, 205))
        elif imagem_botao_1_3 == quadrado_redim:
            screen.blit(imagem_botao_1_3, (435, 205))

        if imagem_botao_1_4 == circulo_redim:
            screen.blit(imagem_botao_1_4, (622, 217))
        elif imagem_botao_1_4 == triangulo_redim:
            screen.blit(imagem_botao_1_4, (608, 205))
        elif imagem_botao_1_4 == quadrado_redim:
            screen.blit(imagem_botao_1_4, (609, 205))

        if imagem_botao_2_1 == circulo_redim:
            screen.blit(imagem_botao_2_1, (100, 361))
        elif imagem_botao_2_1 == triangulo_redim:
            screen.blit(imagem_botao_2_1, (86, 349))
        elif imagem_botao_2_1 == quadrado_redim:
            screen.blit(imagem_botao_2_1, (87, 349))

        if imagem_botao_2_2 == circulo_redim:
            screen.blit(imagem_botao_2_2, (274, 361))
        elif imagem_botao_2_2 == triangulo_redim:
            screen.blit(imagem_botao_2_2, (260, 349))
        elif imagem_botao_2_2 == quadrado_redim:
            screen.blit(imagem_botao_2_2, (261, 349))

        if imagem_botao_2_3 == circulo_redim:
            screen.blit(imagem_botao_2_3, (448, 361))
        elif imagem_botao_2_3 == triangulo_redim:
            screen.blit(imagem_botao_2_3, (434, 349))
        elif imagem_botao_2_3 == quadrado_redim:
            screen.blit(imagem_botao_2_3, (435, 349))

        if imagem_botao_2_4 == circulo_redim:
            screen.blit(imagem_botao_2_4, (622, 361))
        elif imagem_botao_2_4 == triangulo_redim:
            screen.blit(imagem_botao_2_4, (608, 349))
        elif imagem_botao_2_4 == quadrado_redim:
            screen.blit(imagem_botao_2_4, (609, 349))

        if imagem_botao_3_1 == circulo_redim:
            screen.blit(imagem_botao_3_1, (100, 505))
        elif imagem_botao_3_1 == triangulo_redim:
            screen.blit(imagem_botao_3_1, (86, 493))
        elif imagem_botao_3_1 == quadrado_redim:
            screen.blit(imagem_botao_3_1, (87, 493))

        if imagem_botao_3_2 == circulo_redim:
            screen.blit(imagem_botao_3_2, (274, 505))
        elif imagem_botao_3_2 == triangulo_redim:
            screen.blit(imagem_botao_3_2, (260, 493))
        elif imagem_botao_3_2 == quadrado_redim:
            screen.blit(imagem_botao_3_2, (261, 493))

        if imagem_botao_3_3 == circulo_redim:
            screen.blit(imagem_botao_3_3, (448, 505))
        elif imagem_botao_3_3 == triangulo_redim:
            screen.blit(imagem_botao_3_3, (434, 493))
        elif imagem_botao_3_3 == quadrado_redim:
            screen.blit(imagem_botao_3_3, (435, 493))

        if imagem_botao_3_4 == circulo_redim:
            screen.blit(imagem_botao_3_4, (622, 505))
        elif imagem_botao_3_4 == triangulo_redim:
            screen.blit(imagem_botao_3_4, (608, 493))
        elif imagem_botao_3_4 == quadrado_redim:
            screen.blit(imagem_botao_3_4, (609, 493))
 
        # Renderizar e posicionar o texto dos nomes
        fonte = pygame.font.Font(None, 46)
        texto_nome1 = fonte.render(nome_jogador, True, BRANCO)
        posicao_nome1 = (900, 290)
        screen.blit(label_nome1_redim, (855, 265))
        screen.blit(texto_nome1, posicao_nome1)

        texto_nome2 = fonte.render(nome2, True, BRANCO)
        posicao_nome2 = (900, 375)
        screen.blit(label_nome2_redim, (855, 350))
        screen.blit(texto_nome2, posicao_nome2)

        # Resultado do random dos nomes
        if nome_ou_bot_selecionado == nome_jogador:
            texto_nome_selecionado = fonte.render(nome_jogador, True, BRANCO)
            posicao_nome_selecionado = (900, 145)
            screen.blit(label_nome1_redim, (855, 120))
            screen.blit(texto_nome_selecionado, posicao_nome_selecionado)
        if jogador_atual == nome_jogador:
            texto_nome1 = fonte.render(nome_jogador, True, BRANCO)
            posicao_nome1 = (900, 145)
            screen.blit(label_nome1_redim, (855, 120))
            screen.blit(texto_nome1, posicao_nome1)
        elif jogador_atual == 2:
            texto_nome2 = fonte.render(nome2, True, BRANCO)
            posicao_nome2 = (900, 145)
            screen.blit(label_nome2_redim, (855, 120))
            screen.blit(texto_nome2, posicao_nome2)

        else:
            texto_nome_selecionado = fonte.render(nome2, True, BRANCO)
            posicao_nome_selecionado = (900, 145)
            screen.blit(label_nome2_redim, (855, 120))
            screen.blit(texto_nome_selecionado, posicao_nome_selecionado)
            if jogador_atual == 2:
                texto_nome1 = fonte.render(nome_jogador, True, BRANCO)
                posicao_nome1 = (900, 145)
                screen.blit(label_nome1_redim, (855, 120))
                screen.blit(texto_nome1, posicao_nome1)
            elif  jogador_atual == nome_jogador:
                texto_nome2 = fonte.render(nome2, True, BRANCO)
                posicao_nome2 = (900, 145)
                screen.blit(label_nome2_redim, (855, 120))
                screen.blit(texto_nome2, posicao_nome2)

        screen.blit(sair_redim, (1052, 625))
        screen.blit(botao_voltar_redim, (1200, 5))
        pygame.display.update()

        if verificar_vitoria(botao_vazio_redim,imagem_botao_1_1,imagem_botao_1_2,imagem_botao_1_3,imagem_botao_1_4,imagem_botao_2_1,imagem_botao_2_2,imagem_botao_2_3,imagem_botao_2_4,imagem_botao_3_1,imagem_botao_3_2,imagem_botao_3_3,imagem_botao_3_4) == True:
            if jogador_atual == nome_jogador:
                time.sleep(0.5)
                abrir_janela_vitoria_bot(nome2)
            elif jogador_atual == nome2:
                time.sleep(0.5)
                abrir_janela_vitoria_player(nome_jogador)

# jogo 1vs1
def abrir_tabuleiro_1v1(nome_jogador1, nome_jogador2):
    screen = pygame.display.set_mode((screen_width, screen_height))
    jogador_selecionado = random.randint(1,2)
    jogador_atual = 1
    nome_jogador_selecionado = ''

    if jogador_selecionado == 1:
        jogador_atual = 1
        nome_jogador_selecionado = nome_jogador1
    else:
        jogador_atual = 2
        nome_jogador_selecionado = nome_jogador2

    botao_vazio_redim = pygame.Surface((125, 125))
    botao_vazio_redim.fill((166, 124, 74))

    imagem_botao_1_1 = botao_vazio_redim
    imagem_botao_1_2 = botao_vazio_redim
    imagem_botao_1_3 = botao_vazio_redim
    imagem_botao_1_4 = botao_vazio_redim 
    imagem_botao_2_1 = botao_vazio_redim
    imagem_botao_2_2 = botao_vazio_redim
    imagem_botao_2_3 = botao_vazio_redim
    imagem_botao_2_4 = botao_vazio_redim
    imagem_botao_3_1 = botao_vazio_redim
    imagem_botao_3_2 = botao_vazio_redim
    imagem_botao_3_3 = botao_vazio_redim
    imagem_botao_3_4 = botao_vazio_redim

    digitando = True
    while digitando:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                digitando = False
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if sair_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    pygame.quit()
                    quit() 
                elif botao_voltar_redim.get_rect(topleft=(1200, 5)).collidepoint(mouse_pos):
                    digitando = False
                    
                elif botao_tabuleiro_1_1_redim.get_rect(topleft=(80, 214)).collidepoint(mouse_pos):
                    if imagem_botao_1_1 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_1 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_1 = circulo_redim
                            jogador_atual = 1

                    elif imagem_botao_1_1 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_1 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_1 = triangulo_redim
                            jogador_atual = 1
                  
                    elif imagem_botao_1_1 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_1 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_1 = quadrado_redim
                            jogador_atual = 1

                elif botao_tabuleiro_1_2_redim.get_rect(topleft=(254, 214)).collidepoint(mouse_pos):
                    if imagem_botao_1_2 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_2 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_2 = circulo_redim
                            jogador_atual = 1

                    elif imagem_botao_1_2 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_2 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_2 = triangulo_redim
                            jogador_atual = 1
               
                    elif imagem_botao_1_2 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_2 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_2 = quadrado_redim
                            jogador_atual = 1
                
                elif botao_tabuleiro_1_3_redim.get_rect(topleft=(428, 214)).collidepoint(mouse_pos):
                    if imagem_botao_1_3 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_3 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_3 = circulo_redim
                            jogador_atual = 1

                    elif imagem_botao_1_3 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_3 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_3 = triangulo_redim
                            jogador_atual = 1
                      
                    elif imagem_botao_1_3 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_3 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_3 = quadrado_redim
                            jogador_atual = 1
                
                elif botao_tabuleiro_1_4_redim.get_rect(topleft=(602, 214)).collidepoint(mouse_pos):
                    if imagem_botao_1_4 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_4 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_4 = circulo_redim
                            jogador_atual = 1

                    elif imagem_botao_1_4 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_4 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_4 = triangulo_redim
                            jogador_atual = 1
                    
                    elif imagem_botao_1_4 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_1_4 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_1_4 = quadrado_redim
                            jogador_atual = 1

                elif botao_tabuleiro_2_1_redim.get_rect(topleft=(80, 358)).collidepoint(mouse_pos):
                    if imagem_botao_2_1 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_1 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_1 = circulo_redim
                            jogador_atual = 1

                    elif imagem_botao_2_1 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_1 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_1 = triangulo_redim
                            jogador_atual = 1
                   
                    elif imagem_botao_2_1 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_1 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_1 = quadrado_redim
                            jogador_atual = 1
                
                elif botao_tabuleiro_2_2_redim.get_rect(topleft=(254, 358)).collidepoint(mouse_pos):
                    if imagem_botao_2_2 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_2 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_2 = circulo_redim
                            jogador_atual = 1
       
                    elif imagem_botao_2_2 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_2 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_2 = triangulo_redim
                            jogador_atual = 1
                                  
                    elif imagem_botao_2_2 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_2 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_2 = quadrado_redim
                            jogador_atual = 1
                
                elif botao_tabuleiro_2_3_redim.get_rect(topleft=(428, 358)).collidepoint(mouse_pos):
                    if imagem_botao_2_3 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_3 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_3 = circulo_redim
                            jogador_atual = 1
               
                    elif imagem_botao_2_3 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_3 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_3 = triangulo_redim
                            jogador_atual = 1
                                       
                    elif imagem_botao_2_3 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_3 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_3 = quadrado_redim
                            jogador_atual = 1
               
                elif botao_tabuleiro_2_4_redim.get_rect(topleft=(602, 358)).collidepoint(mouse_pos):
                    if imagem_botao_2_4 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_4 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_4 = circulo_redim
                            jogador_atual = 1
                   
                    elif imagem_botao_2_4 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_4 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_4 = triangulo_redim
                            jogador_atual = 1
                                                 
                    elif imagem_botao_2_4 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_2_4 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_2_4 = quadrado_redim
                            jogador_atual = 1
                           
                elif botao_tabuleiro_3_1_redim.get_rect(topleft=(80, 502)).collidepoint(mouse_pos):
                    if imagem_botao_3_1 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_1 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_1 = circulo_redim
                            jogador_atual = 1
                         
                    elif imagem_botao_3_1 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_1 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_1 = triangulo_redim
                            jogador_atual = 1
                                      
                    elif imagem_botao_3_1 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_1 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_1 = quadrado_redim
                            jogador_atual = 1
                   
                elif botao_tabuleiro_3_2_redim.get_rect(topleft=(254, 502)).collidepoint(mouse_pos):
                    if imagem_botao_3_2 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_2 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_2 = circulo_redim
                            jogador_atual = 1
           
                    elif imagem_botao_3_2 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_2 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_2 = triangulo_redim
                            jogador_atual = 1
                          
                    elif imagem_botao_3_2 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_2 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_2 = quadrado_redim
                            jogador_atual = 1
                
                elif botao_tabuleiro_3_3_redim.get_rect(topleft=(428, 502)).collidepoint(mouse_pos):
                    if imagem_botao_3_3 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_3 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_3 = circulo_redim
                            jogador_atual = 1
                 
                    elif imagem_botao_3_3 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_3 = triangulo_redim
                            jogador_atual = 2     
                        elif jogador_atual == 2:
                            imagem_botao_3_3 = triangulo_redim
                              
                    elif imagem_botao_3_3 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_3 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_3 = quadrado_redim
                            jogador_atual = 1

                elif botao_tabuleiro_3_4_redim.get_rect(topleft=(602, 502)).collidepoint(mouse_pos):
                    if imagem_botao_3_4 == botao_vazio_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_4 = circulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_4 = circulo_redim
                            jogador_atual = 1
            
                    elif imagem_botao_3_4 == circulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_4 = triangulo_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_4 = triangulo_redim
                            jogador_atual = 1
                                          
                    elif imagem_botao_3_4 == triangulo_redim:
                        if jogador_atual == 1:
                            imagem_botao_3_4 = quadrado_redim
                            jogador_atual = 2
                        elif jogador_atual == 2:
                            imagem_botao_3_4 = quadrado_redim
                            jogador_atual = 1

        screen.blit(botao_voltar_redim, (1200, 5))
        screen.blit(menu_tabuleiro_redim, (0, 0))

        screen.blit(botao_tabuleiro_1_1_redim, (80, 214))
        screen.blit(botao_tabuleiro_1_2_redim, (254, 214))
        screen.blit(botao_tabuleiro_1_3_redim, (428, 214))
        screen.blit(botao_tabuleiro_1_4_redim, (602, 214))

        screen.blit(botao_tabuleiro_2_1_redim, (80, 358))
        screen.blit(botao_tabuleiro_2_2_redim, (254, 358))
        screen.blit(botao_tabuleiro_2_3_redim, (428, 358))
        screen.blit(botao_tabuleiro_2_4_redim, (602, 358))

        screen.blit(botao_tabuleiro_3_1_redim, (80, 502))
        screen.blit(botao_tabuleiro_3_2_redim, (254, 502))
        screen.blit(botao_tabuleiro_3_3_redim, (428, 502))
        screen.blit(botao_tabuleiro_3_4_redim, (602, 502))

        # Compor estetica/posiçao das peças nos botoes
        if imagem_botao_1_1 == circulo_redim:
            screen.blit(imagem_botao_1_1, (100, 217))
        elif imagem_botao_1_1 == triangulo_redim:
            screen.blit(imagem_botao_1_1, (86, 205))
        elif imagem_botao_1_1 == quadrado_redim:
            screen.blit(imagem_botao_1_1, (87, 205))

        if imagem_botao_1_2 == circulo_redim:
            screen.blit(imagem_botao_1_2, (274, 217))
        elif imagem_botao_1_2 == triangulo_redim:
            screen.blit(imagem_botao_1_2, (260, 205))
        elif imagem_botao_1_2 == quadrado_redim:
            screen.blit(imagem_botao_1_2, (261, 205))

        if imagem_botao_1_3 == circulo_redim:
            screen.blit(imagem_botao_1_3, (448, 217))
        elif imagem_botao_1_3 == triangulo_redim:
            screen.blit(imagem_botao_1_3, (434, 205))
        elif imagem_botao_1_3 == quadrado_redim:
            screen.blit(imagem_botao_1_3, (435, 205))

        if imagem_botao_1_4 == circulo_redim:
            screen.blit(imagem_botao_1_4, (622, 217))
        elif imagem_botao_1_4 == triangulo_redim:
            screen.blit(imagem_botao_1_4, (608, 205))
        elif imagem_botao_1_4 == quadrado_redim:
            screen.blit(imagem_botao_1_4, (609, 205))

        if imagem_botao_2_1 == circulo_redim:
            screen.blit(imagem_botao_2_1, (100, 361))
        elif imagem_botao_2_1 == triangulo_redim:
            screen.blit(imagem_botao_2_1, (86, 349))
        elif imagem_botao_2_1 == quadrado_redim:
            screen.blit(imagem_botao_2_1, (87, 349))

        if imagem_botao_2_2 == circulo_redim:
            screen.blit(imagem_botao_2_2, (274, 361))
        elif imagem_botao_2_2 == triangulo_redim:
            screen.blit(imagem_botao_2_2, (260, 349))
        elif imagem_botao_2_2 == quadrado_redim:
            screen.blit(imagem_botao_2_2, (261, 349))

        if imagem_botao_2_3 == circulo_redim:
            screen.blit(imagem_botao_2_3, (448, 361))
        elif imagem_botao_2_3 == triangulo_redim:
            screen.blit(imagem_botao_2_3, (434, 349))
        elif imagem_botao_2_3 == quadrado_redim:
            screen.blit(imagem_botao_2_3, (435, 349))

        if imagem_botao_2_4 == circulo_redim:
            screen.blit(imagem_botao_2_4, (622, 361))
        elif imagem_botao_2_4 == triangulo_redim:
            screen.blit(imagem_botao_2_4, (608, 349))
        elif imagem_botao_2_4 == quadrado_redim:
            screen.blit(imagem_botao_2_4, (609, 349))

        if imagem_botao_3_1 == circulo_redim:
            screen.blit(imagem_botao_3_1, (100, 505))
        elif imagem_botao_3_1 == triangulo_redim:
            screen.blit(imagem_botao_3_1, (86, 493))
        elif imagem_botao_3_1 == quadrado_redim:
            screen.blit(imagem_botao_3_1, (87, 493))

        if imagem_botao_3_2 == circulo_redim:
            screen.blit(imagem_botao_3_2, (274, 505))
        elif imagem_botao_3_2 == triangulo_redim:
            screen.blit(imagem_botao_3_2, (260, 493))
        elif imagem_botao_3_2 == quadrado_redim:
            screen.blit(imagem_botao_3_2, (261, 493))

        if imagem_botao_3_3 == circulo_redim:
            screen.blit(imagem_botao_3_3, (448, 505))
        elif imagem_botao_3_3 == triangulo_redim:
            screen.blit(imagem_botao_3_3, (434, 493))
        elif imagem_botao_3_3 == quadrado_redim:
            screen.blit(imagem_botao_3_3, (435, 493))

        if imagem_botao_3_4 == circulo_redim:
            screen.blit(imagem_botao_3_4, (622, 505))
        elif imagem_botao_3_4 == triangulo_redim:
            screen.blit(imagem_botao_3_4, (608, 493))
        elif imagem_botao_3_4 == quadrado_redim:
            screen.blit(imagem_botao_3_4, (609, 493))
        
        # Renderizar e posicionar o texto dos nomes
        fonte = pygame.font.Font(None, 46)
        texto_nome1 = fonte.render(nome_jogador1, True, BRANCO)
        posicao_nome1 = (900, 290)
        screen.blit(label_nome1_redim, (855, 265))
        screen.blit(texto_nome1, posicao_nome1)

        texto_nome2 = fonte.render(nome_jogador2, True, BRANCO)
        posicao_nome2 = (900, 375)
        screen.blit(label_nome2_redim, (855, 350))
        screen.blit(texto_nome2, posicao_nome2)

        # Resultado do random dos nomes
        if jogador_selecionado == 1:
            texto_nome_selecionado = fonte.render(nome_jogador1, True, BRANCO)
            posicao_nome_selecionado = (900, 145)
            screen.blit(label_nome1_redim, (855, 120))
            screen.blit(texto_nome_selecionado,posicao_nome_selecionado)
            if jogador_atual == 1:
                texto_nome1 = fonte.render(nome_jogador1, True, BRANCO)
                posicao_nome1 = (900, 145)
                screen.blit(label_nome1_redim, (855, 120))
                screen.blit(texto_nome1, posicao_nome1)
            elif jogador_atual == 2:
                texto_nome2 = fonte.render(nome_jogador2, True, BRANCO)
                posicao_nome2 = (900, 145)
                screen.blit(label_nome2_redim, (855, 120))
                screen.blit(texto_nome2, posicao_nome2)
        elif jogador_selecionado == 2:
            texto_nome_selecionado = fonte.render(nome_jogador2, True, BRANCO)
            posicao_nome_selecionado = (900, 145)
            screen.blit(label_nome2_redim, (855, 120))
            screen.blit(texto_nome_selecionado, posicao_nome_selecionado)
            if jogador_atual == 1:
                texto_nome1 = fonte.render(nome_jogador1, True, BRANCO)
                posicao_nome1 = (900, 145)
                screen.blit(label_nome1_redim, (855, 120))
                screen.blit(texto_nome1, posicao_nome1)
            elif jogador_atual == 2:
                texto_nome2 = fonte.render(nome_jogador2, True, BRANCO)
                posicao_nome2 = (900, 145)
                screen.blit(label_nome2_redim, (855, 120))
                screen.blit(texto_nome2, posicao_nome2)

        screen.blit(sair_redim, (1052, 625))
        screen.blit(botao_voltar_redim, (1200, 5))
        pygame.display.update()

        if verificar_vitoria(botao_vazio_redim,imagem_botao_1_1,imagem_botao_1_2,imagem_botao_1_3,imagem_botao_1_4,imagem_botao_2_1,imagem_botao_2_2,imagem_botao_2_3,imagem_botao_2_4,imagem_botao_3_1,imagem_botao_3_2,imagem_botao_3_3,imagem_botao_3_4) == True:
            if jogador_atual == 1:
                time.sleep(0.5)
                abrir_janela_vitoria_p2(nome_jogador2)
            elif jogador_atual == 2:
                time.sleep(0.5)
                abrir_janela_vitoria_p1(nome_jogador1)

# verificar vitoria no tabuleiro
def verificar_vitoria(botao_vazio_redim,imagem_botao_1_1,imagem_botao_1_2,imagem_botao_1_3,imagem_botao_1_4,imagem_botao_2_1,imagem_botao_2_2,imagem_botao_2_3,imagem_botao_2_4,imagem_botao_3_1,imagem_botao_3_2,imagem_botao_3_3,imagem_botao_3_4):

    # Verificar linhas
    if imagem_botao_1_1 == imagem_botao_1_2 == imagem_botao_1_3 != botao_vazio_redim:
        return True
    elif imagem_botao_1_2 == imagem_botao_1_3 == imagem_botao_1_4 != botao_vazio_redim:
        return True
    elif imagem_botao_2_1 == imagem_botao_2_2 == imagem_botao_2_3 != botao_vazio_redim:
        return True
    elif imagem_botao_2_2 == imagem_botao_2_3 == imagem_botao_2_4 != botao_vazio_redim:
        return True
    elif imagem_botao_3_1 == imagem_botao_3_2 == imagem_botao_3_3 != botao_vazio_redim:
        return True
    elif imagem_botao_3_2 == imagem_botao_3_3 == imagem_botao_3_4 != botao_vazio_redim:
        return True

    # Verificar colunas
    elif imagem_botao_1_1 == imagem_botao_2_1 == imagem_botao_3_1 != botao_vazio_redim:
        return True
    elif imagem_botao_1_2 == imagem_botao_2_2 == imagem_botao_3_2 != botao_vazio_redim:
        return True
    elif imagem_botao_1_3 == imagem_botao_2_3 == imagem_botao_3_3 != botao_vazio_redim:
        return True
    elif imagem_botao_1_4 == imagem_botao_2_4 == imagem_botao_3_4 != botao_vazio_redim:
        return True

    # Verificar diagonais
    elif imagem_botao_1_1 == imagem_botao_2_2 == imagem_botao_3_3 != botao_vazio_redim:
        return True
    elif imagem_botao_1_2 == imagem_botao_2_3 == imagem_botao_3_4 != botao_vazio_redim:
        return True
    elif imagem_botao_3_1 == imagem_botao_2_2 == imagem_botao_1_3 != botao_vazio_redim:
        return True
    elif imagem_botao_3_2 == imagem_botao_2_3 == imagem_botao_1_4 != botao_vazio_redim:
        return True

    # Nenhuma vitória
    return False

# menus de vitoria
def abrir_janela_vitoria_p1(nome_jogador1):
    screen = pygame.display.set_mode((screen_width, screen_height))
    label_nome1_redim = pygame.transform.scale(label_nome1, (531,123))
    
    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
                pygame.quit()
                quit()
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_menu_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    abrir_menu_jogo()
                    return

        screen.blit(menu_vitoria_redim, (0, 0))

        fonte = pygame.font.Font(None, 48)
        texto_nome1 = fonte.render(nome_jogador1, True, BRANCO)
        posicao_nome1 = (440, 417)
        screen.blit(label_nome1_redim,(370,370))
        screen.blit(texto_nome1, posicao_nome1)
        
        screen.blit(botao_menu_redim, (1052, 625))
        pygame.display.update()

def abrir_janela_vitoria_p2(nome_jogador2):
    screen = pygame.display.set_mode((screen_width, screen_height))
    label_nome2_redim = pygame.transform.scale(label_nome2, (531,123))

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
                pygame.quit()
                quit()
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_menu_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    abrir_menu_jogo()
                    return

        screen.blit(menu_vitoria_redim, (0, 0))

        fonte = pygame.font.Font(None, 48)
        texto_nome2 = fonte.render(nome_jogador2, True, BRANCO)
        posicao_nome2 = (440, 417)
        screen.blit(label_nome2_redim, (370, 370))
        screen.blit(texto_nome2, posicao_nome2)

        screen.blit(botao_menu_redim, (1052, 625))
        pygame.display.update()

def abrir_janela_vitoria_player(nome_jogador):
    screen = pygame.display.set_mode((screen_width, screen_height))
    label_nome1_redim = pygame.transform.scale(label_nome1, (531,123))

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
                pygame.quit()
                quit()
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_menu_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    abrir_menu_jogo()
                    return

        screen.blit(menu_vitoria_redim, (0, 0))

        fonte = pygame.font.Font(None, 48)
        texto_nome = fonte.render(nome_jogador, True, BRANCO)
        posicao_nome = (440, 417)
        screen.blit(label_nome1_redim, (370, 370))
        screen.blit(texto_nome, posicao_nome)

        screen.blit(botao_menu_redim, (1052, 625))
        pygame.display.update()

def abrir_janela_vitoria_bot(nome2):
    screen = pygame.display.set_mode((screen_width, screen_height))
    label_nome2_redim = pygame.transform.scale(label_nome2, (531,123))

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
                pygame.quit()
                quit()
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if botao_menu_redim.get_rect(topleft=(1052, 625)).collidepoint(mouse_pos):
                    abrir_menu_jogo()
                    return

        screen.blit(menu_vitoria_redim, (0, 0))

        fonte = pygame.font.Font(None, 48)
        texto_nome2 = fonte.render(nome2, True, BRANCO)
        posicao_nome2 = (440, 417)
        screen.blit(label_nome2_redim, (370, 370))
        screen.blit(texto_nome2, posicao_nome2)

        screen.blit(botao_menu_redim, (1052, 625))
        pygame.display.update()

# menu principal com as opções de jogo
def abrir_menu_jogo():
    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

            # detectar quando o utilizador clica nos botões
            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_pos = pygame.mouse.get_pos()
                if sair_redim.get_rect(topleft=(1052,625)).collidepoint(mouse_pos):
                    pygame.quit()
                    quit()
                elif comecar_redim.get_rect(topleft=(500, 250)).collidepoint(mouse_pos):
                    abrir_janela_comecar()
                elif carregar_redim.get_rect(topleft=(500, 375)).collidepoint(mouse_pos):
                    print("Botão CARREGAR foi clicado!")
                elif regras_redim.get_rect(topleft=(500, 500)).collidepoint(mouse_pos):
                    abrir_janela_regras()

        # apresentar os botoes no ecra
        screen.blit(menu_redim, (0,0))
        screen.blit(sair_redim, (1052,625))
        screen.blit(comecar_redim, (500, 250))
        screen.blit(carregar_redim, (500, 372))
        screen.blit(regras_redim, (500,500))
        pygame.display.update()

############## MAIN ################
running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
        
        # Abrir o menu do jogo se clicar no rato ou no teclado
        elif event.type == pygame.MOUSEBUTTONDOWN:
            mouse_pos = pygame.mouse.get_pos()
            abrir_menu_jogo()
        elif event.type == pygame.KEYDOWN:
            abrir_menu_jogo()
    
    screen.blit(pagina_inicial_redim, (0,0))
    pygame.display.update()