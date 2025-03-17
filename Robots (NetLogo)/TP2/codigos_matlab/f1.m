function y = f1(x)
    y = 4 * (sin(5 * pi * x + 0.5).^6) .* exp(log2((x - 0.8).^2));
end