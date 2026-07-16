import type { Metadata } from "next";

import "./globals.css";

export const metadata: Metadata = {
  title: "Abertura de conta | Cloud Labs Bank",
  description: "Inicie sua jornada de abertura de conta.",
};

interface RootLayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({
  children,
}: Readonly<RootLayoutProps>) {
  return (
    <html lang="pt-BR">
      <body>{children}</body>
    </html>
  );
}