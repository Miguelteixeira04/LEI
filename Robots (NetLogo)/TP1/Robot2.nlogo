globals [
  pollution-level              ; nível de poluição
  max-cleaner-debris           ; capacidade máxima de detritos que o Cleaner pode carregar
  cleaner-energy-blue          ; energia do Cleaner azul
  current-debris-count-blue    ; contagem de detritos que o Cleaner azul está a transportar
  cleaner-energy-black         ; energia do Cleaner preto
  current-debris-count-black   ; contagem de detritos que o Cleaner preto está a transportar
  total-debris-count           ; contagem total de detritos que o cleaner ja apanhou
  tick-count                   ; contador de ticks

]

turtles-own [
  role                  ; papel do agente (Cleaner ou Polluter)
  debris-type           ; tipo de detrito que o Polluter pode depositar
  cleaner-type          ; tipo de cleaner (azul ou preto)
  charger-assigned     ; o charger atribuído a cada Cleaner

]

;-------------------------------------------------------------------------------------------------
; Criação de Agentes (Cleaner e Polluters)
to setup
  clear-all
  ask patches [
    set pcolor green    ; todas as células começam limpas (verde)
  ]
  set pollution-level 0                      ; nivel de poluição inicial
  set max-cleaner-debris num-cleaner-cap     ; capacidade máxima de detritos

  set cleaner-energy-blue energy             ; energia inicial do Cleaner
  set current-debris-count-blue 0            ; inicializa a contagem de detritos
  set cleaner-energy-black 0                 ; energia inicial do Cleaner preto
  set current-debris-count-black 0

  set tick-count 0

   ; Primeiro cria o Trash
   create-turtles num_trash [               ; vai criar o número do trashes que tiver no deslizador
    set color grey
    set role "Trash"
    set shape "target"
    setxy random-xcor random-ycor
    limit                                    ; Garante inicializam dentro dos limites
    safe-spawn                               ; Garante que não nasçam sobre o Charger ou outros Trash
    set size 2
  ]
   ;Criar os Chargers
  create-turtles num-cleaners [
    set color yellow         ; Charger
    set role "Charger"
    set shape "lightning"    ; Usar a forma personalizada com o raio amarelo
    set size 2               ; Define o tamanho

    ; Use um contador para colocar cada charger em uma posição específica
    if count turtles with [role = "Charger"] = 1 [
      setxy -15.5 -15.5      ; Primeiro charger
    ]
    if count turtles with [role = "Charger"] = 2 [
      setxy 15.5 15.5        ; Segundo charger
      ;set cleaner-energy-black energy
    ]
  ]

 ; Criar os Cleaners e associá-los aos Chargers correspondentes
  create-turtles num-cleaners  [
    set role "Cleaner"
    set shape "circle"     ; Define a forma do Cleaner como círculo
    set size 2

    if count turtles with [role = "Cleaner"] = 1 [
      setxy -15.5 -15.5               ; Primeiro charger
      set color blue                  ; cor azul
      set cleaner-type "blue"         ; cleaner type azul
      set cleaner-energy-blue energy  ; define a energia do azul consuante a energia definida
      set charger-assigned one-of turtles with [role = "Charger" and xcor = -15.5 and ycor = -15.5]  ; Charger específico para azul
    ]
    if count turtles with [role = "Cleaner"] = 2 [
      setxy 15.5 15.5                  ; Segundo charger
      set color black
      set cleaner-type "black"         ; cleaner type preto
      set cleaner-energy-black energy  ; define a energia do preto consuante a energia definida
      set charger-assigned one-of turtles with [role = "Charger" and xcor = 15.5 and ycor = 15.5]  ; Charger específico para preto
    ]
  ]

  ; Criar 3 Polluters, cada um com um tipo de detrito exclusivo
  create-turtles 1 [
    set color red    ; Polluter 1
    set role "Polluter"
    set debris-type 0  ; Tipo 0 de resíduo (vermelho)
    set shape "square_red"
    safe-spawn
    setxy random-xcor random-ycor
    limit

  ]

  create-turtles 1 [
    set color pink  ; Polluter 2
    set role "Polluter"
    set debris-type 1  ; Tipo 1 de resíduo (amarelo)
    set shape "square_pink"
    safe-spawn
    setxy random-xcor random-ycor
    limit
  ]

  create-turtles 1 [
    set color orange  ; Polluter 3
    set role "Polluter"
    set debris-type 2  ; Tipo 2 de resíduo (laranja)
    set shape "square_orange"
    safe-spawn
    setxy random-xcor random-ycor
    limit
  ]

  reset-timer
end

;-------------------------------------------------------------------------------------------------
;Movimentação e Comportamento
to go_once
  ask turtles [
    move
    if role = "Polluter" [
      deposit-debris
    ]
    if role = "Cleaner" [
      clean
    ]
  ]
  set tick-count tick-count + 1

  ; Atualiza os monitores após cada tick
  update-monitors
end

to go_n
  let n n-movimentos ; pega o valor do deslizador
  repeat n [
    go_once
  ]
end


to go
  while [cleaner-energy-blue > 0 or cleaner-energy-black > 0] [
    go_once
  ]
  stop
end


;-------------------------------------------------------------------------------------------------
;Função movimento do Cleaner e dos Polluters
to move
  let trash one-of turtles with [role = "Trash"]
  let charger one-of turtles with [role = "Charger"]

  if role = "Polluter" [
    ; Evita o Trash e o Charger
    let heading-away-from-trash (ifelse-value (distance trash < 2) [subtract-headings towards trash 180] [heading])
    let heading-away-from-charger (ifelse-value (distance charger < 2) [subtract-headings towards charger 180] [heading-away-from-trash])
    set heading heading-away-from-charger
    move-in-four-directions
  ]

  if role = "Cleaner" [
    if cleaner-type = "blue" [
      set cleaner-energy-blue cleaner-energy-blue - 1
      mover-cleaner-blue trash charger
    ]

    if cleaner-type = "black" [
      set cleaner-energy-black cleaner-energy-black - 1
      mover-cleaner-black trash charger
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
; Movimentação e ações do cleaner azul
to mover-cleaner-blue [trash charger]
  if current-debris-count-blue = max-cleaner-debris [
    if cleaner-energy-blue >= 45 [
      move-to-trash-blue
    ]
    if cleaner-energy-blue < 45 [
      if distance trash < 5 [
        move-to-trash-blue
        if current-debris-count-blue = 0 [
          move-to-charger-blue
          if distance charger < 1 [recharge-blue]
        ]
      ]
      if distance trash >= 5 [
        move-to-charger-blue
        if distance charger < 1 [recharge-blue]
      ]
    ]
  ]
  if current-debris-count-blue < max-cleaner-debris [
    if cleaner-energy-blue >= 30 [
      scan-and-collect-blue
      move-in-four-directions
    ]
    if cleaner-energy-blue < 30 [
      move-to-charger-blue
      if distance charger < 1 [recharge-blue]
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
; Movimentação e ações do cleaner preto
to mover-cleaner-black [trash charger]
  if current-debris-count-black = max-cleaner-debris [
    if cleaner-energy-black >= 45 [
      move-to-trash-black
    ]
    if cleaner-energy-black < 45 [
      if distance trash < 5 [
        move-to-trash-black
        if current-debris-count-black = 0 [
          move-to-charger-black
          if distance charger < 1 [recharge-black]
        ]
      ]
      if distance trash >= 5 [
        move-to-charger-black
        if distance charger < 1 [recharge-black]
      ]
    ]
  ]
  if current-debris-count-black < max-cleaner-debris [
    if cleaner-energy-black >= 30 [
      scan-and-collect-black
      move-in-four-directions
    ]
    if cleaner-energy-black < 30 [
      move-to-charger-black
      if distance charger < 1 [recharge-black]
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
; Função para nao andar na diagonal e nao andar contra a parede
to move-in-four-directions
  let possible-directions [0 90 180 270]  ; Direções possíveis
  let chosen-direction one-of possible-directions  ; Escolhe uma direção aleatória

  ; Impedir movimento em direção à parede
  let isAtWall false

  ; Verificar se está em uma parede
  if xcor >= 15.5 [
    if chosen-direction = 90 [ set isAtWall true ] ; Direção para a direita
  ]
  if xcor <= -15.5 [
    if chosen-direction = 270 [ set isAtWall true ] ; Direção para a esquerda
  ]
  if ycor >= 15.5 [
    if chosen-direction = 0 [ set isAtWall true ] ; Direção para cima
  ]
  if ycor <= -15.5 [
    if chosen-direction = 180 [ set isAtWall true ] ; Direção para baixo
  ]

  ; Se a direção aleatória estiver próxima de uma parede, escolher uma nova direção
  while [isAtWall] [
    set chosen-direction one-of possible-directions  ; Escolhe uma nova direção

    ; Reinicializa a verificação
    set isAtWall false
    if xcor >= 15.5 [
      if chosen-direction = 90 [ set isAtWall true ]
    ]
    if xcor <= -15.5 [
      if chosen-direction = 270 [ set isAtWall true ]
    ]
    if ycor >= 15.5 [
      if chosen-direction = 0 [ set isAtWall true ]
    ]
    if ycor <= -15.5 [
      if chosen-direction = 180 [ set isAtWall true ]
    ]
  ]

  ; Define a nova direção e move-se para frente
  set heading chosen-direction  ; Define a nova direção
  fd 1  ; Move-se para frente
end

;-------------------------------------------------------------------------------------------------
; Função para mover o Cleaner em direção ao Trash
to move-to-trash-blue
  let nearest-trash min-one-of turtles with [role = "Trash"] [distance myself]

  if abs (xcor - [xcor] of nearest-trash) > abs (ycor - [ycor] of nearest-trash) [
    if xcor < [xcor] of nearest-trash [ set heading 90 ]
    if xcor > [xcor] of nearest-trash [ set heading 270 ]
  ]
  if abs (xcor - [xcor] of nearest-trash) <= abs (ycor - [ycor] of nearest-trash) [
    if ycor < [ycor] of nearest-trash [ set heading 0 ]
    if ycor > [ycor] of nearest-trash [ set heading 180 ]
  ]

  fd 1

  if distance nearest-trash < 0.7 [
    set current-debris-count-blue 0
    set pollution-level pollution-level - max-cleaner-debris
    scan-and-collect-blue
    wait 1
    if cleaner-energy-blue < 45 [ move-to-charger-blue ]
  ]
end

;-------------------------------------------------------------------------------------------------
to move-to-trash-black
  let nearest-trash min-one-of turtles with [role = "Trash"] [distance myself]

  if abs (xcor - [xcor] of nearest-trash) > abs (ycor - [ycor] of nearest-trash) [
    if xcor < [xcor] of nearest-trash [ set heading 90 ]
    if xcor > [xcor] of nearest-trash [ set heading 270 ]
  ]
  if abs (xcor - [xcor] of nearest-trash) <= abs (ycor - [ycor] of nearest-trash) [
    if ycor < [ycor] of nearest-trash [ set heading 0 ]
    if ycor > [ycor] of nearest-trash [ set heading 180 ]
  ]

  fd 1

  if distance nearest-trash < 0.7 [
    set current-debris-count-black 0
    set pollution-level pollution-level - max-cleaner-debris
    scan-and-collect-black
    wait 1
    if cleaner-energy-black < 45 [ move-to-charger-black ]
  ]
end

;-------------------------------------------------------------------------------------------------
; Função para mover o Cleaner em direção ao Charger
to move-to-charger-blue
  let charger charger-assigned
  if abs (xcor - [xcor] of charger) > abs (ycor - [ycor] of charger) [
    if xcor < [xcor] of charger [ set heading 90 ]
    if xcor > [xcor] of charger [ set heading 270 ]
  ]
  if abs (xcor - [xcor] of charger) <= abs (ycor - [ycor] of charger) [
    if ycor < [ycor] of charger [ set heading 0 ]
    if ycor > [ycor] of charger [ set heading 180 ]
  ]

  fd 1
end

;-------------------------------------------------------------------------------------------------
to move-to-charger-black
  ; Agora vai diretamente para o charger associado ao cleaner azul
  let charger charger-assigned

  if abs (xcor - [xcor] of charger) > abs (ycor - [ycor] of charger) [
    if xcor < [xcor] of charger [ set heading 90 ]
    if xcor > [xcor] of charger [ set heading 270 ]
  ]
  if abs (xcor - [xcor] of charger) <= abs (ycor - [ycor] of charger) [
    if ycor < [ycor] of charger [ set heading 0 ]
    if ycor > [ycor] of charger [ set heading 180 ]
  ]

  fd 1
end

;-------------------------------------------------------------------------------------------------
; Função de carregamento do cleaner
to recharge-blue
  while [cleaner-energy-blue < 100] [
    set cleaner-energy-blue cleaner-energy-blue + charging-rate
    if cleaner-energy-blue > 100 [ set cleaner-energy-blue 100 ]
    update-monitors
    ask turtles with [role = "Polluter"] [ move ]
    wait temp-carregamento
  ]
end
;-------------------------------------------------------------------------------------------------
to recharge-black
  while [cleaner-energy-black < 100] [
    set cleaner-energy-black cleaner-energy-black + charging-rate
    if cleaner-energy-black > 100 [ set cleaner-energy-black 100 ]
    update-monitors
    ask turtles with [role = "Polluter"] [ move ]
    wait temp-carregamento
  ]
end

;-------------------------------------------------------------------------------------------------
; Função que delimita o mapa
to limit
 ; Verifica os es das coordenadas
  if xcor > 15.5 [ set xcor 15.5 ] ; e direito
  if xcor < -15.5 [ set xcor -15.5 ] ; e esquerdo
  if ycor > 15.5 [ set ycor 15.5 ] ; e superior
  if ycor < -15.5 [ set ycor -15.5 ] ; e inferior

end

;-------------------------------------------------------------------------------------------------
;Depósito de resíduos apanhados pelo cleaner
to deposit-debris
  if debris-type = 0 [
    if random-float 1 < red-polluter-probability and pcolor != red [
      set pcolor red  ; muda a cor da célula para vermelho (tipo 0)
      set pollution-level pollution-level + 1
    ]
  ]
  if debris-type = 1 [
    if random-float 1 < pink-polluter-probability and pcolor != pink [
      set pcolor pink  ; muda a cor da célula para amarelo (tipo 1)
      set pollution-level pollution-level + 1
    ]
  ]
  if debris-type = 2 [
    if random-float 1 < orange-polluter-probability and pcolor != orange [
      set pcolor orange  ; muda a cor da célula para laranja (tipo 2)

      set pollution-level pollution-level + 1
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
; Função para o Cleaner fazer scan e coletar
to scan-and-collect-blue
  let radius 1  ; Definir o raio
  let debris-patches patches with [pcolor = red or pcolor = pink or pcolor = orange] in-radius radius

  ; Verifica se há detritos nas proximidades
  if any? debris-patches [
    ; Encontra o patch mais próximo com detritos
    let nearest-debris-patch min-one-of debris-patches [distance myself]

    ; Calcula a direção para o patch de detritos mais próximo
    face nearest-debris-patch

    ; Move-se em direção ao patch mais próximo
    fd 1

    ; Recolhe os detritos se estiver no patch correto
    if (pcolor = red or pcolor = pink or pcolor = orange) and (current-debris-count-blue < max-cleaner-debris) [
      set pcolor green
      set current-debris-count-blue current-debris-count-blue + 1
      set pollution-level pollution-level - 1
      set total-debris-count total-debris-count + 1
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
to scan-and-collect-black
  let radius 1
  let nearby-patches patches in-radius radius

  if pcolor != green [
    let debris-patches patches with [pcolor = red or pcolor = pink or pcolor = orange]

    if any? debris-patches [
      let most-debris-patch max-one-of debris-patches [count turtles-here]
      move-to most-debris-patch
    ]
  ]

  ask patches in-radius radius [
    if (pcolor = red or pcolor = pink or pcolor = orange) and (current-debris-count-black < max-cleaner-debris) [
      set pcolor green
      set current-debris-count-black current-debris-count-black + 1
      set pollution-level pollution-level - 1
      set total-debris-count total-debris-count + 1
    ]
  ]
end

;-------------------------------------------------------------------------------------------------
;Limpeza de resíduos pelo Cleaner
to clean
  if (pcolor = red or pcolor = pink or pcolor = orange) and (current-debris-count-blue  < max-cleaner-debris) [
    set pcolor green  ; limpa a célula (volta a ser verde)
    set current-debris-count-blue  current-debris-count-blue  + 1  ; conta os detritos coletados
    set pollution-level pollution-level - 1  ; diminui o nível de poluição
    set total-debris-count total-debris-count + 1

  ]
end

;-------------------------------------------------------------------------------------------------
; Atualização dos Monitores
to update-monitors
  ; Atualiza os monitores do nível de poluição, energia e detritos
  set-current-plot "Evolução da Limpeza"
  plot (current-debris-count-blue + current-debris-count-black) ; Sumariza os detritos dos dois Cleaners
  set-current-plot "Evolução da Contaminação"
  plot pollution-level
end

;-----------------------------------------------------------------------------------------------
;Não darem spawn em cima do charger e/ou trash
to safe-spawn
  let trash-turtles turtles with [role = "Trash"]
  let charger one-of turtles with [role = "Charger"]
  ; Verifica se o Charger foi criado
  while [any? other turtles in-radius 4 or (charger != nobody and distance charger < 4)] [
    limit
    setxy random-xcor random-ycor
    limit
  ]
end
@#$#@#$#@
GRAPHICS-WINDOW
210
10
647
448
-1
-1
13.0
1
10
1
1
1
0
0
0
1
-16
16
-16
16
0
0
1
ticks
30.0

BUTTON
71
42
148
75
Setup
setup
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
71
231
134
264
Go
go
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
71
90
148
123
Go_Once
Go_once
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

BUTTON
71
138
148
171
Go_N
Go_n\n
NIL
1
T
OBSERVER
NIL
NIL
NIL
NIL
1

SLIDER
21
280
193
313
energy
energy
10
100
100.0
1
1
%
HORIZONTAL

MONITOR
673
28
836
73
Nível de poluição
pollution-level
17
1
11

MONITOR
673
80
836
125
Energia do Cleaner
cleaner-energy-blue
17
1
11

MONITOR
673
130
836
175
Contagem atual de detritos
current-debris-count-blue
17
1
11

PLOT
224
453
424
603
Evolução da contaminação
Tempo (ticks)
Nível Poluição
0.0
10.0
0.0
10.0
true
false
"" ""
PENS
"default" 1.0 0 -13345367 true "" "plot current-debris-count-blue\n"
"pen-1" 1.0 0 -16777216 true "" "plot current-debris-count-black\n"

PLOT
437
453
638
603
Evolução da limpeza
Tempo (ticks)
Cont Detritos
0.0
10.0
0.0
10.0
true
false
"" ""
PENS
"default" 1.0 0 -16777216 true "" "plot pollution-level"

SLIDER
21
322
193
355
red-polluter-probability
red-polluter-probability
0
100
100.0
1
1
%
HORIZONTAL

SLIDER
21
362
193
395
pink-polluter-probability
pink-polluter-probability
0
100
100.0
1
1
%
HORIZONTAL

SLIDER
22
402
194
435
orange-polluter-probability
orange-polluter-probability
0
100
100.0
1
1
%
HORIZONTAL

SLIDER
676
285
839
318
num-cleaner-cap
num-cleaner-cap
2
100
100.0
1
1
detritos
HORIZONTAL

SLIDER
676
330
839
363
charging-rate
charging-rate
0
100
50.0
1
1
%
HORIZONTAL

SLIDER
19
184
190
217
n-movimentos
n-movimentos
0
100
50.0
1
1
NIL
HORIZONTAL

MONITOR
673
180
836
225
Total de detritos apanhados
total-debris-count
17
1
11

SLIDER
23
441
194
474
num_trash
num_trash
2
10
2.0
1
1
NIL
HORIZONTAL

SLIDER
675
372
840
405
num-cleaners
num-cleaners
1
2
2.0
1
1
NIL
HORIZONTAL

SLIDER
675
241
838
274
temp-carregamento
temp-carregamento
1
10
2.0
1
1
s
HORIZONTAL

MONITOR
864
78
1016
123
Energia do Cleaner Preto
cleaner-energy-black
17
1
11

MONITOR
853
136
1062
181
Contagem atual de detrtios (preto)
current-debris-count-black
17
1
11

MONITOR
866
24
978
69
Contador de Ticks
tick-count
17
1
11

@#$#@#$#@
## WHAT IS IT?

(a general understanding of what the model is trying to show or explain)

## HOW IT WORKS

(what rules the agents use to create the overall behavior of the model)

## HOW TO USE IT

(how to use the model, including a description of each of the items in the Interface tab)

## THINGS TO NOTICE

(suggested things for the user to notice while running the model)

## THINGS TO TRY

(suggested things for the user to try to do (move sliders, switches, etc.) with the model)

## EXTENDING THE MODEL

(suggested things to add or change in the Code tab to make the model more complicated, detailed, accurate, etc.)

## NETLOGO FEATURES

(interesting or unusual features of NetLogo that the model uses, particularly in the Code tab; or where workarounds were needed for missing features)

## RELATED MODELS

(models in the NetLogo Models Library and elsewhere which are of related interest)

## CREDITS AND REFERENCES

(a reference to the model's URL on the web if it has one, as well as any other necessary credits, citations, and links)
@#$#@#$#@
default
true
0
Polygon -7500403 true true 150 5 40 250 150 205 260 250

airplane
true
0
Polygon -7500403 true true 150 0 135 15 120 60 120 105 15 165 15 195 120 180 135 240 105 270 120 285 150 270 180 285 210 270 165 240 180 180 285 195 285 165 180 105 180 60 165 15

arrow
true
0
Polygon -7500403 true true 150 0 0 150 105 150 105 293 195 293 195 150 300 150

box
false
0
Polygon -7500403 true true 150 285 285 225 285 75 150 135
Polygon -7500403 true true 150 135 15 75 150 15 285 75
Polygon -7500403 true true 15 75 15 225 150 285 150 135
Line -16777216 false 150 285 150 135
Line -16777216 false 150 135 15 75
Line -16777216 false 150 135 285 75

bug
true
0
Circle -7500403 true true 96 182 108
Circle -7500403 true true 110 127 80
Circle -7500403 true true 110 75 80
Line -7500403 true 150 100 80 30
Line -7500403 true 150 100 220 30

butterfly
true
0
Polygon -7500403 true true 150 165 209 199 225 225 225 255 195 270 165 255 150 240
Polygon -7500403 true true 150 165 89 198 75 225 75 255 105 270 135 255 150 240
Polygon -7500403 true true 139 148 100 105 55 90 25 90 10 105 10 135 25 180 40 195 85 194 139 163
Polygon -7500403 true true 162 150 200 105 245 90 275 90 290 105 290 135 275 180 260 195 215 195 162 165
Polygon -16777216 true false 150 255 135 225 120 150 135 120 150 105 165 120 180 150 165 225
Circle -16777216 true false 135 90 30
Line -16777216 false 150 105 195 60
Line -16777216 false 150 105 105 60

car
false
0
Polygon -7500403 true true 300 180 279 164 261 144 240 135 226 132 213 106 203 84 185 63 159 50 135 50 75 60 0 150 0 165 0 225 300 225 300 180
Circle -16777216 true false 180 180 90
Circle -16777216 true false 30 180 90
Polygon -16777216 true false 162 80 132 78 134 135 209 135 194 105 189 96 180 89
Circle -7500403 true true 47 195 58
Circle -7500403 true true 195 195 58

circle
false
0
Circle -7500403 true true 0 0 300

circle 2
false
0
Circle -7500403 true true 0 0 300
Circle -16777216 true false 30 30 240

cow
false
0
Polygon -7500403 true true 200 193 197 249 179 249 177 196 166 187 140 189 93 191 78 179 72 211 49 209 48 181 37 149 25 120 25 89 45 72 103 84 179 75 198 76 252 64 272 81 293 103 285 121 255 121 242 118 224 167
Polygon -7500403 true true 73 210 86 251 62 249 48 208
Polygon -7500403 true true 25 114 16 195 9 204 23 213 25 200 39 123

cylinder
false
0
Circle -7500403 true true 0 0 300

dot
false
0
Circle -7500403 true true 90 90 120

face happy
false
0
Circle -7500403 true true 8 8 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Polygon -16777216 true false 150 255 90 239 62 213 47 191 67 179 90 203 109 218 150 225 192 218 210 203 227 181 251 194 236 217 212 240

face neutral
false
0
Circle -7500403 true true 8 7 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Rectangle -16777216 true false 60 195 240 225

face sad
false
0
Circle -7500403 true true 8 8 285
Circle -16777216 true false 60 75 60
Circle -16777216 true false 180 75 60
Polygon -16777216 true false 150 168 90 184 62 210 47 232 67 244 90 220 109 205 150 198 192 205 210 220 227 242 251 229 236 206 212 183

fish
false
0
Polygon -1 true false 44 131 21 87 15 86 0 120 15 150 0 180 13 214 20 212 45 166
Polygon -1 true false 135 195 119 235 95 218 76 210 46 204 60 165
Polygon -1 true false 75 45 83 77 71 103 86 114 166 78 135 60
Polygon -7500403 true true 30 136 151 77 226 81 280 119 292 146 292 160 287 170 270 195 195 210 151 212 30 166
Circle -16777216 true false 215 106 30

flag
false
0
Rectangle -7500403 true true 60 15 75 300
Polygon -7500403 true true 90 150 270 90 90 30
Line -7500403 true 75 135 90 135
Line -7500403 true 75 45 90 45

flower
false
0
Polygon -10899396 true false 135 120 165 165 180 210 180 240 150 300 165 300 195 240 195 195 165 135
Circle -7500403 true true 85 132 38
Circle -7500403 true true 130 147 38
Circle -7500403 true true 192 85 38
Circle -7500403 true true 85 40 38
Circle -7500403 true true 177 40 38
Circle -7500403 true true 177 132 38
Circle -7500403 true true 70 85 38
Circle -7500403 true true 130 25 38
Circle -7500403 true true 96 51 108
Circle -16777216 true false 113 68 74
Polygon -10899396 true false 189 233 219 188 249 173 279 188 234 218
Polygon -10899396 true false 180 255 150 210 105 210 75 240 135 240

house
false
0
Rectangle -7500403 true true 45 120 255 285
Rectangle -16777216 true false 120 210 180 285
Polygon -7500403 true true 15 120 150 15 285 120
Line -16777216 false 30 120 270 120

leaf
false
0
Polygon -7500403 true true 150 210 135 195 120 210 60 210 30 195 60 180 60 165 15 135 30 120 15 105 40 104 45 90 60 90 90 105 105 120 120 120 105 60 120 60 135 30 150 15 165 30 180 60 195 60 180 120 195 120 210 105 240 90 255 90 263 104 285 105 270 120 285 135 240 165 240 180 270 195 240 210 180 210 165 195
Polygon -7500403 true true 135 195 135 240 120 255 105 255 105 285 135 285 165 240 165 195

lightning
false
4
Rectangle -1184463 true true 0 0 450 315
Polygon -7500403 true false 120 135 90 195 135 195 105 300 225 165 180 165 210 105 165 105 195 0 75 135

line
true
0
Line -7500403 true 150 0 150 300

line half
true
0
Line -7500403 true 150 0 150 150

novo_target
false
0
Rectangle -7500403 true true 0 0 345 300
Circle -7500403 true true 450 360 300
Circle -16777216 true false 45 15 240
Circle -7500403 true true 75 45 180
Circle -16777216 true false 105 75 120
Circle -7500403 true true 135 105 60

pentagon
false
0
Polygon -7500403 true true 150 15 15 120 60 285 240 285 285 120

person
false
0
Circle -7500403 true true 110 5 80
Polygon -7500403 true true 105 90 120 195 90 285 105 300 135 300 150 225 165 300 195 300 210 285 180 195 195 90
Rectangle -7500403 true true 127 79 172 94
Polygon -7500403 true true 195 90 240 150 225 180 165 105
Polygon -7500403 true true 105 90 60 150 75 180 135 105

plant
false
0
Rectangle -7500403 true true 135 90 165 300
Polygon -7500403 true true 135 255 90 210 45 195 75 255 135 285
Polygon -7500403 true true 165 255 210 210 255 195 225 255 165 285
Polygon -7500403 true true 135 180 90 135 45 120 75 180 135 210
Polygon -7500403 true true 165 180 165 210 225 180 255 120 210 135
Polygon -7500403 true true 135 105 90 60 45 45 75 105 135 135
Polygon -7500403 true true 165 105 165 135 225 105 255 45 210 60
Polygon -7500403 true true 135 90 120 45 150 15 180 45 165 90

sheep
false
15
Circle -1 true true 203 65 88
Circle -1 true true 70 65 162
Circle -1 true true 150 105 120
Polygon -7500403 true false 218 120 240 165 255 165 278 120
Circle -7500403 true false 214 72 67
Rectangle -1 true true 164 223 179 298
Polygon -1 true true 45 285 30 285 30 240 15 195 45 210
Circle -1 true true 3 83 150
Rectangle -1 true true 65 221 80 296
Polygon -1 true true 195 285 210 285 210 240 240 210 195 210
Polygon -7500403 true false 276 85 285 105 302 99 294 83
Polygon -7500403 true false 219 85 210 105 193 99 201 83

square
false
14
Rectangle -7500403 true false 30 30 270 270
Rectangle -2674135 true false 30 30 270 270
Rectangle -16777216 false true 30 30 270 270
Rectangle -16777216 true true 30 30 45 270
Rectangle -16777216 true true 30 30 270 45
Rectangle -16777216 true true 255 30 270 270
Rectangle -16777216 true true 45 255 270 270

square 2
false
0
Rectangle -7500403 true true 30 30 270 270
Rectangle -16777216 true false 60 60 240 240

square_orange
false
15
Rectangle -7500403 true false 30 30 270 270
Rectangle -2674135 true false 30 30 270 270
Rectangle -16777216 false false 30 30 270 270
Rectangle -16777216 true false 30 30 45 270
Rectangle -16777216 true false 30 30 270 45
Rectangle -16777216 true false 255 30 270 270
Rectangle -16777216 true false 45 255 270 270
Rectangle -2064490 false false 45 45 255 255
Rectangle -2064490 true false 45 45 255 255
Rectangle -2064490 true false 45 45 255 255
Rectangle -2674135 true false 45 45 255 255
Rectangle -2674135 true false 45 45 255 255
Rectangle -6459832 true false 45 45 255 255
Rectangle -955883 true false 45 45 255 255

square_pink
false
13
Rectangle -7500403 true false 30 30 270 270
Rectangle -2674135 true false 30 30 270 270
Rectangle -16777216 false false 30 30 270 270
Rectangle -16777216 true false 30 30 45 270
Rectangle -16777216 true false 30 30 270 45
Rectangle -16777216 true false 255 30 270 270
Rectangle -16777216 true false 45 255 270 270
Rectangle -2064490 false true 45 45 255 255
Rectangle -2064490 true true 45 45 255 255
Rectangle -2064490 true true 45 45 255 255
Rectangle -2674135 true false 45 45 255 255
Rectangle -2674135 true false 45 45 255 255
Rectangle -2064490 true true 45 45 255 255

square_red
false
13
Rectangle -7500403 true false 30 30 270 270
Rectangle -2674135 true false 30 30 270 270
Rectangle -16777216 false false 30 30 270 270
Rectangle -16777216 true false 30 30 45 270
Rectangle -16777216 true false 30 30 270 45
Rectangle -16777216 true false 255 30 270 270
Rectangle -16777216 true false 45 255 270 270
Rectangle -2064490 false true 45 45 255 255
Rectangle -2064490 true true 45 45 255 255
Rectangle -2064490 true true 45 45 255 255
Rectangle -2674135 true false 45 45 255 255
Rectangle -2674135 true false 45 45 255 255

star
false
0
Polygon -7500403 true true 151 1 185 108 298 108 207 175 242 282 151 216 59 282 94 175 3 108 116 108

target
false
0
Rectangle -7500403 true true 0 0 345 300
Circle -7500403 true true 450 360 300
Circle -16777216 true false 30 30 240
Circle -7500403 true true 60 60 180
Circle -16777216 true false 90 90 120
Circle -7500403 true true 120 120 60

tree
false
0
Circle -7500403 true true 118 3 94
Rectangle -6459832 true false 120 195 180 300
Circle -7500403 true true 65 21 108
Circle -7500403 true true 116 41 127
Circle -7500403 true true 45 90 120
Circle -7500403 true true 104 74 152

triangle
false
0
Polygon -7500403 true true 150 30 15 255 285 255

triangle 2
false
0
Polygon -7500403 true true 150 30 15 255 285 255
Polygon -16777216 true false 151 99 225 223 75 224

truck
false
0
Rectangle -7500403 true true 4 45 195 187
Polygon -7500403 true true 296 193 296 150 259 134 244 104 208 104 207 194
Rectangle -1 true false 195 60 195 105
Polygon -16777216 true false 238 112 252 141 219 141 218 112
Circle -16777216 true false 234 174 42
Rectangle -7500403 true true 181 185 214 194
Circle -16777216 true false 144 174 42
Circle -16777216 true false 24 174 42
Circle -7500403 false true 24 174 42
Circle -7500403 false true 144 174 42
Circle -7500403 false true 234 174 42

turtle
true
0
Polygon -10899396 true false 215 204 240 233 246 254 228 266 215 252 193 210
Polygon -10899396 true false 195 90 225 75 245 75 260 89 269 108 261 124 240 105 225 105 210 105
Polygon -10899396 true false 105 90 75 75 55 75 40 89 31 108 39 124 60 105 75 105 90 105
Polygon -10899396 true false 132 85 134 64 107 51 108 17 150 2 192 18 192 52 169 65 172 87
Polygon -10899396 true false 85 204 60 233 54 254 72 266 85 252 107 210
Polygon -7500403 true true 119 75 179 75 209 101 224 135 220 225 175 261 128 261 81 224 74 135 88 99

wheel
false
0
Circle -7500403 true true 3 3 294
Circle -16777216 true false 30 30 240
Line -7500403 true 150 285 150 15
Line -7500403 true 15 150 285 150
Circle -7500403 true true 120 120 60
Line -7500403 true 216 40 79 269
Line -7500403 true 40 84 269 221
Line -7500403 true 40 216 269 79
Line -7500403 true 84 40 221 269

wolf
false
0
Polygon -16777216 true false 253 133 245 131 245 133
Polygon -7500403 true true 2 194 13 197 30 191 38 193 38 205 20 226 20 257 27 265 38 266 40 260 31 253 31 230 60 206 68 198 75 209 66 228 65 243 82 261 84 268 100 267 103 261 77 239 79 231 100 207 98 196 119 201 143 202 160 195 166 210 172 213 173 238 167 251 160 248 154 265 169 264 178 247 186 240 198 260 200 271 217 271 219 262 207 258 195 230 192 198 210 184 227 164 242 144 259 145 284 151 277 141 293 140 299 134 297 127 273 119 270 105
Polygon -7500403 true true -1 195 14 180 36 166 40 153 53 140 82 131 134 133 159 126 188 115 227 108 236 102 238 98 268 86 269 92 281 87 269 103 269 113

x
false
0
Polygon -7500403 true true 270 75 225 30 30 225 75 270
Polygon -7500403 true true 30 75 75 30 270 225 225 270
@#$#@#$#@
NetLogo 6.4.0
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
@#$#@#$#@
default
0.0
-0.2 0 0.0 1.0
0.0 1 1.0 0.0
0.2 0 0.0 1.0
link direction
true
0
Line -7500403 true 150 150 90 180
Line -7500403 true 150 150 210 180
@#$#@#$#@
0
@#$#@#$#@
